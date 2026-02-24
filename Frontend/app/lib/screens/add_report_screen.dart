import 'package:flutter/material.dart';
import 'package:geolocator/geolocator.dart';
import 'package:geocoding/geocoding.dart';
import 'package:image_picker/image_picker.dart';
import 'package:dio/dio.dart';
import 'dart:io';
import '../services/auth_service.dart';
import '../services/lang_service.dart';

class AddReportScreen extends StatefulWidget {
  const AddReportScreen({super.key});

  @override
  State<AddReportScreen> createState() => _AddReportScreenState();
}

class _AddReportScreenState extends State<AddReportScreen> {
  final _titleController = TextEditingController();
  final _descController = TextEditingController();
  final _dio = Dio();
  
  String _address = LangService.t("gps_wait");
  bool _isLocating = true;
  bool _isSending = false;
  Position? _currentPosition;
  File? _image;

  @override
  void initState() {
    super.initState();
    _determinePosition();
  }

  Future<void> _determinePosition() async {
    setState(() => _isLocating = true);
    try {
      LocationPermission permission = await Geolocator.checkPermission();
      if (permission == LocationPermission.denied) permission = await Geolocator.requestPermission();

      Position position = await Geolocator.getCurrentPosition(locationSettings: const LocationSettings(accuracy: LocationAccuracy.high));
      _currentPosition = position;

      try {
        await setLocaleIdentifier(LangService.currentLang == "uz" ? "uz_UZ" : "ru_RU");
        List<Placemark> placemarks = await placemarkFromCoordinates(position.latitude, position.longitude);
        if (placemarks.isNotEmpty) {
          Placemark p = placemarks[0];
          setState(() {
            _address = "${p.locality}, ${p.street}, ${p.name}";
            _isLocating = false;
          });
        }
      } catch (e) {
        setState(() {
          _address = "Lat: ${position.latitude.toStringAsFixed(4)}, Long: ${position.longitude.toStringAsFixed(4)}";
          _isLocating = false;
        });
      }
    } catch (e) {
      setState(() { _address = "GPS Error"; _isLocating = false; });
    }
  }

  Future<void> _pickImage() async {
    final picker = ImagePicker();
    final pickedFile = await picker.pickImage(source: ImageSource.camera, imageQuality: 50);
    if (pickedFile != null) setState(() => _image = File(pickedFile.path));
  }

  Future<void> _sendReport() async {
    if (_image == null || _titleController.text.isEmpty) {
      _showSnackBar(LangService.t("need_photo_title"), Colors.orange);
      return;
    }

    setState(() => _isSending = true);
    try {
      String deviceId = await AuthService().getUniqueId();
      String fileName = _image!.path.split('/').last;
      
      FormData formData = FormData.fromMap({
        "file": await MultipartFile.fromFile(_image!.path, filename: fileName),
      });

      var uploadRes = await _dio.post("http://136.113.150.143:5001/api/Reports/upload-file", data: formData);
      String fileId = uploadRes.data["fileId"].toString();

      // Фикс 400: Поля с большой буквы для .NET
      await _dio.post("http://136.113.150.143:5001/api/Reports", data: {
        "Title": _titleController.text,
        "Description": _descController.text,
        "DeviceId": deviceId,
        "Latitude": _currentPosition?.latitude ?? 0.0,
        "Longitude": _currentPosition?.longitude ?? 0.0,
        "ImageHash": fileId,
      });

      if (!mounted) return;
      _showSnackBar(LangService.t("report_sent"), Colors.green);
      Navigator.pop(context);
    } catch (e) {
      if (e is DioException) _showSnackBar("Xato: ${e.response?.data ?? e.message}", Colors.red);
    } finally {
      setState(() => _isSending = false);
    }
  }

  void _showSnackBar(String m, Color c) => ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text(m), backgroundColor: c));

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text(LangService.t("new_report")), backgroundColor: const Color(0xFF0A56C4), foregroundColor: Colors.white),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(20),
        child: Column(
          children: [
            _buildLocationCard(),
            const SizedBox(height: 20),
            _buildPhotoPicker(),
            const SizedBox(height: 20),
            // Убрали фильтры, чтобы русский текст вводился свободно
            _buildInput(_titleController, LangService.t("report_title"), Icons.report_problem),
            const SizedBox(height: 15),
            _buildInput(_descController, LangService.t("description"), Icons.notes, maxLines: 3),
            const SizedBox(height: 30),
            _isSending ? const CircularProgressIndicator() : _buildSubmitButton(),
          ],
        ),
      ),
    );
  }

  Widget _buildLocationCard() {
    return Container(
      padding: const EdgeInsets.all(15),
      decoration: BoxDecoration(color: Colors.blue.withOpacity(0.05), borderRadius: BorderRadius.circular(15), border: Border.all(color: Colors.blue.withOpacity(0.2))),
      child: Row(children: [Icon(Icons.location_on, color: _isLocating ? Colors.grey : Colors.blue), const SizedBox(width: 10), Expanded(child: Text(_address, style: const TextStyle(fontSize: 13)))]),
    );
  }

  Widget _buildPhotoPicker() {
    return GestureDetector(
      onTap: _pickImage,
      child: Container(
        height: 180, width: double.infinity,
        decoration: BoxDecoration(color: Colors.grey[100], borderRadius: BorderRadius.circular(20), border: Border.all(color: Colors.grey[300]!), image: _image != null ? DecorationImage(image: FileImage(_image!), fit: BoxFit.cover) : null),
        child: _image == null ? Column(mainAxisAlignment: MainAxisAlignment.center, children: [const Icon(Icons.camera_alt, size: 50, color: Colors.grey), Text(LangService.t("take_photo"))]) : null,
      ),
    );
  }

  Widget _buildInput(TextEditingController ctrl, String hint, IconData icon, {int maxLines = 1}) {
    return TextField(
      controller: ctrl, 
      maxLines: maxLines,
      // Удаляем любые inputFormatters, чтобы не блокировать кириллицу
      decoration: InputDecoration(prefixIcon: Icon(icon, color: const Color(0xFF0A56C4)), hintText: hint, filled: true, fillColor: Colors.white, border: OutlineInputBorder(borderRadius: BorderRadius.circular(15))),
    );
  }

  Widget _buildSubmitButton() {
    return SizedBox(width: double.infinity, height: 55, child: ElevatedButton(onPressed: _isLocating ? null : _sendReport, style: ElevatedButton.styleFrom(backgroundColor: const Color(0xFF0A56C4), shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15))), child: Text(LangService.t("send"), style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold))));
  }
}