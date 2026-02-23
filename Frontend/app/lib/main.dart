import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'screens/login_screen.dart'; // Импортируем наш будущий экран

void main() {
  runApp(const GeoportalApp());
}

class GeoportalApp extends StatelessWidget {
  const GeoportalApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Geoportal Uzbekistan',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        // Используем современный Material 3
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.blueAccent),
        useMaterial3: true,
        // Ставим шрифт, чтобы всё было стильно
        textTheme: GoogleFonts.montserratTextTheme(),
      ),
      home: const LoginScreen(),
    );
  }
}