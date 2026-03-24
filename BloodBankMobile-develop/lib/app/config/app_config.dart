class AppConfig {
  final String appName;
  final String backendUrl;

  AppConfig({
    required this.appName,
    required this.backendUrl,
  });

  // Development environment - use 10.0.2.2 for Android emulator, localhost for iOS simulator
  // For physical devices, use your computer's IP address (e.g., http://192.168.1.x:5076/)
  factory AppConfig.dev() => AppConfig(
        appName: 'BloodBankMobile',
        backendUrl:
            'http://192.168.1.131:5076/', // Sử dụng IP máy tính để thiết bị thật có thể truy cập
      );

  // Production environment - replace with your actual server URL
  factory AppConfig.prod() => AppConfig(
        appName: 'BloodBankMobile',
        backendUrl:
            'http://your-server-ip:5076/', // Thay đổi IP và port của server tại đây
      );
}
