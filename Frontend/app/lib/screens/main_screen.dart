import 'package:flutter/material.dart';
import '../services/auth_service.dart';
import '../services/lang_service.dart';
import 'add_report_screen.dart';
import 'dart:io';

class MainScreen extends StatefulWidget {
  const MainScreen({super.key});

  @override
  State<MainScreen> createState() => _MainScreenState();
}

class _MainScreenState extends State<MainScreen> {
  int _selectedIndex = 0;
  int _myReportsCount = 0;
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _loadStats();
  }

void detectLanguage() {
  String systemLocale = Platform.localeName.split('_')[0];
  if (systemLocale == "ru" || systemLocale == "uz") {
    AppLang.current = systemLocale;
  } else {
    AppLang.current = "uz";
  }
}
  Future<void> _loadStats() async {
    setState(() => _isLoading = true);
    final count = await AuthService().getReportCount();
    if (mounted) {
      setState(() {
        _myReportsCount = count;
        _isLoading = false;
      });
    }
  }

  // Список страниц
  List<Widget> get _pages => [
    _buildDashboardContent(),
    const Center(child: Text("Xarita yaqin orada... / Карта скоро...")),
    _buildSettingsPage(),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Container(
        decoration: const BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
            colors: [Color(0xFF0A56C4), Color(0xFF3AB0C2), Color(0xFF3FE543)],
          ),
        ),
        child: SafeArea(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _buildHeader(),
              Expanded(
                child: Container(
                  decoration: const BoxDecoration(
                    color: Color(0xFFF8F9FA),
                    borderRadius: BorderRadius.only(
                      topLeft: Radius.circular(30),
                      topRight: Radius.circular(30),
                    ),
                  ),
                  child: IndexedStack(
                    index: _selectedIndex,
                    children: _pages,
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _selectedIndex,
        onTap: (index) => setState(() => _selectedIndex = index),
        selectedItemColor: const Color(0xFF0A56C4),
        items: [
          BottomNavigationBarItem(icon: const Icon(Icons.home), label: LangService.t("home")),
          BottomNavigationBarItem(icon: const Icon(Icons.map), label: LangService.t("map")),
          BottomNavigationBarItem(icon: const Icon(Icons.settings), label: LangService.t("profile")),
        ],
      ),
    );
  }

  Widget _buildHeader() {
    return Padding(
      padding: const EdgeInsets.all(25.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text("Geoportal", style: TextStyle(color: Colors.white, fontSize: 28, fontWeight: FontWeight.bold)),
          const SizedBox(height: 8),
          Text(LangService.t("monitoring"), style: const TextStyle(color: Colors.white70, fontSize: 14)),
        ],
      ),
    );
  }

  Widget _buildDashboardContent() {
    return RefreshIndicator(
      onRefresh: _loadStats,
      child: ListView(
        padding: const EdgeInsets.all(20),
        children: [
          Text(LangService.t("stats_region"), style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
          const SizedBox(height: 20),
          Row(
            children: [
              _buildStatCard(LangService.t("my_reports"), _isLoading ? "..." : "$_myReportsCount", Colors.blue),
              const SizedBox(width: 15),
              _buildStatCard(LangService.t("resolved"), "0", Colors.green),
            ],
          ),
          const SizedBox(height: 30),
          _buildActionCard(Icons.map_outlined, LangService.t("open_map"), LangService.t("view_nearby"), () {}),
          _buildActionCard(Icons.add_a_photo_outlined, LangService.t("new_report"), LangService.t("fix_violation"), () async {
            await Navigator.push(context, MaterialPageRoute(builder: (context) => const AddReportScreen()));
            _loadStats();
          }),
        ],
      ),
    );
  }

  Widget _buildSettingsPage() {
    return Padding(
      padding: const EdgeInsets.all(20),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(LangService.t("profile"), style: const TextStyle(fontSize: 22, fontWeight: FontWeight.bold)),
          const SizedBox(height: 20),
          ListTile(
            leading: const Icon(Icons.language, color: Color(0xFF0A56C4)),
            title: const Text("Tilni o'zgartirish / Сменить язык"),
            subtitle: Text(LangService.currentLang == "uz" ? "O'zbekcha 🇺🇿" : "Русский 🇷🇺"),
            onTap: () => setState(() => LangService.currentLang = LangService.currentLang == "uz" ? "ru" : "uz"),
          ),
          const Divider(),
          FutureBuilder(
            future: AuthService().getUniqueId(),
            builder: (context, snap) => ListTile(
              leading: const Icon(Icons.phone_android, color: Colors.grey),
              title: const Text("Device ID"),
              subtitle: Text(snap.data ?? "..."),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStatCard(String label, String value, Color color) {
    return Expanded(
      child: Container(
        padding: const EdgeInsets.all(20),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(20),
          boxShadow: [BoxShadow(color: color.withOpacity(0.1), blurRadius: 10)],
          border: Border.all(color: color.withOpacity(0.2)),
        ),
        child: Column(
          children: [
            Text(value, style: TextStyle(fontSize: 26, fontWeight: FontWeight.bold, color: color)),
            Text(label, textAlign: TextAlign.center, style: const TextStyle(color: Colors.grey, fontSize: 12)),
          ],
        ),
      ),
    );
  }

  Widget _buildActionCard(IconData icon, String title, String subtitle, VoidCallback onTap) {
    return Card(
      margin: const EdgeInsets.only(bottom: 15),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
      child: ListTile(
        leading: Icon(icon, color: const Color(0xFF0A56C4)),
        title: Text(title, style: const TextStyle(fontWeight: FontWeight.bold)),
        subtitle: Text(subtitle, style: const TextStyle(fontSize: 12)),
        trailing: const Icon(Icons.arrow_forward_ios, size: 14),
        onTap: onTap,
      ),
    );
  }
}