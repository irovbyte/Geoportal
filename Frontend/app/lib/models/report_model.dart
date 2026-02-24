class Report {
  final String id;
  final String description;
  final double latitude;
  final double longitude;
  final String imageHash;

  Report({required this.id, required this.description, required this.latitude, required this.longitude, required this.imageHash});

  factory Report.fromJson(Map<String, dynamic> json) {
    return Report(
      id: json['id'] ?? '',
      description: json['description'] ?? '',
      latitude: (json['latitude'] as num).toDouble(),
      longitude: (json['longitude'] as num).toDouble(),
      imageHash: json['imageHash'] ?? '',
    );
  }
}