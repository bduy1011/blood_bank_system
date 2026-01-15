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
        backendUrl: 'http://10.0.2.2:5076/', // 10.0.2.2 is the special IP for Android emulator to access host machine
      );

  // Production environment - replace with your actual server URL
  factory AppConfig.prod() => AppConfig(
        appName: 'BloodBankMobile',
        backendUrl: 'http://your-server-ip:5076/', // Thay đổi IP và port của server tại đây
      );
}
