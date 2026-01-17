// ignore_for_file: constant_identifier_names

import 'package:flutter/material.dart';
import 'package:flutter_localization/flutter_localization.dart';

final FlutterLocalization localization = FlutterLocalization.instance;

enum AppLanguage {
  en,
  vi;

  String get languageCode {
    switch (this) {
      case AppLanguage.en:
        return "en";
      case AppLanguage.vi:
        return "vi";
    }
  }

  MapLocale get defaultLocale {
    switch (this) {
      case AppLanguage.en:
        return MapLocale(languageCode, AppLocale.EN, countryCode: "US");
      case AppLanguage.vi:
        return MapLocale(languageCode, AppLocale.VI, countryCode: "VN");
    }
  }
}

extension StringExt on String {
  String translate(BuildContext context) {
    return getString(context);
  }
}

mixin class AppLocale {
  // Home
  static const String homeBook = 'home_book';
  static const String homeRegister = 'home_register';
  static const String homeHistory = 'home_history';
  static const String homeLocation = 'home_location';
  static const String homeBloodUsers = 'home_blood_users';
  static const String homeStatictics = 'home_statictics';
  static const String homeManagement = 'home_management';
  static const String search = 'search';
  static const String settings = 'settings';
  static const String generalSetting = 'generalSetting';
  static const String myAccount = 'my_account';
  static const String restorePurchased = 'restore_purchased';
  static const String enableNotifications = 'enable_notifications';
  static const String all_premium_access = 'all_premium_access';
  static const String supportFeedback = 'support_feedback';
  static const String howToUseGuide = 'how_to_use_guide';
  static const String rateUs = 'rate_us';
  static const String feedbackSupport = 'feedback_support';
  static const String moreAboutUs = 'more_about_us';
  static const String aboutUs = 'about_us';
  static const String share = 'share';
  static const String privacyPolicy = 'privacy_policy';
  static const String bloodDonationHistory = 'blood_donation_history';
  static const String bloodDonationRegister = 'blood_donation_register';
  static const String friend = 'friend';
  static const String listFriend = 'listFriend';
  static const String group = 'group';
  static const String joinGroup = 'join_group';
  static const String deleteAccount = 'delete_account';
  static const String logout = 'logout';
  static const String bloodType = "blood_type";
  static const String times = 'times';
  static const String filter = 'filter';
  static const String provinceCity = 'province_city';
  static const String enterProvinceCity = 'enter_province_city';
  static const String ward = 'ward';
  static const String district = 'district';
  static const String enterDistrict = 'enter_district';
  static const String fromDate = 'from_date';
  static const String today = 'today';
  static const String tomorrow = 'tomorrow';
  static const String enterTime = 'enter_time';
  static const String toDay = 'to_day';
  static const String location = 'location';
  static const String application = 'application';
  static const String news = 'news';
  static const String all = 'all';
  static const String dontHaveAnAccount = 'dont_have_an_account';
  static const String loginByOtp = 'login_by_otp';
  static const String login = 'login';
  static const String contact = 'contact';
  static const String signUp = 'sign_up';
  static const String username = 'username';
  static const String password = 'password';
  static const String oldPassword = 'oldPassword';
  static const String confirmPassword = 'confirm_password';
  static const String loginWithGoogle = 'login_with_google';
  static const String loginFail = 'loginFail';
  static const String authLoginWith = "auth_loginWith";
  static const String formRequiredPasswordError =
      "form_required_password_error";
  static const String formRequiredUsernameError =
      "form_required_username_error";
  static const String formRequiredNameError = "form_required_name_error";
  static const String formRequiredPhoneError = "form_required_phone_error";
  static const String road = 'road';
  static const String register = 'register';
  static const String registerDonateBlood = 'registerDonateBlood';
  static const String registerBuyBlood = 'registerBuyBlood';
  static const String approveBuyBlood = 'approveBuyBlood';
  static const String bloodDonationSchedule = 'bloodDonationSchedule';
  static const String next = 'next';
  static const String prev = 'prev';
  static const String name = 'name';
  static const String gender = 'gender';
  static const String updateInformation = 'updateInformation';
  static const String fullname = 'fullname';
  static const String birthYear = 'birthYear';
  static const String dateOfBirth = 'dateOfBirth';
  static const String idCard = 'idCard';
  static const String phoneNumber = 'phoneNumber';
  static const String email = 'email';
  static const String notification = 'notification';
  static const String notificationUpdateProfile = 'notificationUpdateProfile';
  static const String cancel = 'cancel';
  static const String accept = 'accept';
  static const String yes = 'yes';
  static const String no = 'no';
  static const String dominance = 'dominance';
  static const String influence = 'influence';
  static const String steadiness = 'steadiness';
  static const String compliance = 'compliance';
  static const String personalityGroup = 'personalityGroup';
  static const String questionAndAnswer = 'questionAndAnswer';
  static const String listRegister = 'listRegister';
  static const String selectBloodBuyUnit = 'selectBloodBuyUnit';
  static const String note = 'note';
  static const String accountInformation = 'account_information';
  static const String changePassword = 'change_password';
  static const String appInformation = 'app_information';
  static const String versionInformation = 'version_information';
  static const String setup = 'setup';
  static const String language = 'language';
  static const String english = 'english';
  static const String vietnamese = 'vietnamese';
  static const String thankYouMessage = 'thank_you_message';
  static const String confirmDeleteAccount = 'confirm_delete_account';
  static const String deleteAccountMessage = 'delete_account_message';
  static const String confirmLogout = 'confirm_logout';
  static const String logoutMessage = 'logout_message';
  static const String confirmExitRegisterDonateBlood =
      'confirm_exit_register_donate_blood';
  static const String confirmExitRegisterDonateBloodMessage =
      'confirm_exit_register_donate_blood_message';
  static const String confirmRegisterWithYesAnswer =
      'confirm_register_with_yes_answer';
  static const String confirmRegisterWithYesAnswerMessage =
      'confirm_register_with_yes_answer_message';
  static const String logoutFailed = 'logout_failed';
  static const String deleteAccountSuccess = 'delete_account_success';
  static const String deleteAccountFailed = 'delete_account_failed';
  static const String scanQRCCCD = 'scan_qr_cccd';
  static const String tryAgain = 'try_again';
  static const String back = 'back';
  static const String close = 'close';
  static const String cameraError = 'camera_error';
  static const String cameraInitError = 'camera_init_error';
  static const String qrScanInstruction = 'qr_scan_instruction';
  static const String cannotInitCamera = 'cannot_init_camera';
  static const String error = 'error';
  static const String cannotReadQRCode = 'cannot_read_qr_code';
  static const String passwordNotMatch = 'password_not_match';
  static const String registerAccountSuccess = 'register_account_success';
  static const String registerAccountFailed = 'register_account_failed';
  static const String pleaseSignBeforeConfirm = 'please_sign_before_confirm';
  static const String confirm = 'confirm';
  static const String registerAccount = 'register_account';
  static const String signatureConfirmed = 'signature_confirmed';
  static const String passwordMinLength = 'password_min_length';
  static const String cannotSaveSignature = 'cannot_save_signature';
  static const String errorOccurred = 'error_occurred';
  static const String signatureCancelled = 'signature_cancelled';
  static const String functions = 'functions';
  static const String home = 'home';
  static const String personal = 'personal';
  static const String historyRegisterBuyBlood = 'history_register_buy_blood';
  static const String approveBuyBloodRequest = 'approve_buy_blood_request';
  static const String contactSupport = 'contact_support';
  static const String registerBuyBloodShort = 'register_buy_blood_short';
  static const String bloodDonationScheduleShort =
      'blood_donation_schedule_short';
  static const String bloodDonationHistoryShort =
      'blood_donation_history_short';
  static const String bloodDonationRegisterShort =
      'blood_donation_register_short';
  static const String yourAccount = 'your_account';
  static const String softwareInformation = 'software_information';
  static const String noNotificationYet = 'no_notification_yet';
  static const String notUpdatePersonalInfo = 'not_update_personal_info';
  static const String updateNow = 'update_now';
  static const String noData = 'no_data';
  static const String pleaseUpdatePersonalInfoBefore =
      'please_update_personal_info_before';
  static const String expectedArrivalDateTime = 'expected_arrival_date_time';
  static const String contactAddress = 'contact_address';
  static const String fromToDate = 'from_to_date';
  static const String pleaseEnterApprovalQuantity =
      'please_enter_approval_quantity';
  static const String pleaseEnterFullInfo = 'please_enter_full_info';
  static const String pleaseEnterFullNameBeforeSign =
      'please_enter_full_name_before_sign';
  static const String pleaseEnterIdCardBeforeSign =
      'please_enter_id_card_before_sign';
  static const String pleaseEnterFeedbackContent =
      'please_enter_feedback_content';
  static const String pleaseUpdatePersonalInfoBeforeFeedback =
      'please_update_personal_info_before_feedback';
  static const String pleaseUpdatePersonalInfoBeforeRegister =
      'please_update_personal_info_before_register';
  static const String pleaseUpdatePersonalInfoBeforeBuyBlood =
      'please_update_personal_info_before_buy_blood';
  static const String pleaseSelect = 'pleaseSelect';
  static const String pleaseEnter = 'pleaseEnter';
  static const String pleaseWait = 'pleaseWait';
  static const String pleaseUpdateAppVersion = 'pleaseUpdateAppVersion';
  static const String newVersionAvailable = 'newVersionAvailable';
  static const String saveChanges = 'save_changes';
  static const String yearOfBirth = 'yearOfBirth';
  static const String male = 'male';
  static const String female = 'female';
  static const String expectedArrivalDate = 'expectedArrivalDate';
  static const String occupation = 'occupation';
  static const String company = 'company';
  static const String youAreRegisteringToDonate = 'youAreRegisteringToDonate';
  static const String timeFromDate = 'timeFromDate';
  static const String selectDateTime = 'select_date_time';
  static const String selectBloodDonationSchedule =
      'selectBloodDonationSchedule';
  static const String registeredCount = 'registeredCount';
  static const String resendCode = 'resendCode';
  static const String usernameWithIdCard = 'usernameWithIdCard';
  static const String notEnterOldPassword = 'not_enter_old_password';
  static const String notEnterPassword = 'not_enter_password';
  static const String changePasswordSuccess = 'change_password_success';
  static const String changePasswordFailed = 'change_password_failed';
  static const String numberOfBloodDonations = 'number_of_blood_donations';
  static const String welcomeBack = 'welcome_back';
  static const String notDonatedYet = 'not_donated_yet';
  static const String daysSinceLastDonation = 'days_since_last_donation';
  static const String lastDonation = 'last_donation';
  static const String notificationTitle = 'notification_title';
  static const String cannotFindRoute = 'cannot_find_route';
  static const String pleaseSelectBloodUnit = 'please_select_blood_unit';
  static const String approveBuyBloodSuccess = 'approve_buy_blood_success';
  static const String rejectBuyBloodSuccess = 'reject_buy_blood_success';
  static const String sendSuccess = 'send_success';
  static const String notEnterFullName = 'not_enter_full_name';
  static const String invalidFullName = 'invalid_full_name';
  static const String notEnterUsername = 'not_enter_username';
  static const String notEnterPhone = 'not_enter_phone';
  static const String invalidPhone = 'invalid_phone';
  static const String qrCodeReadSuccess = 'qr_code_read_success';
  static const String qrCodeReadError = 'qr_code_read_error';
  static const String qrScanError = 'qr_scan_error';
  static const String signatureSavedSuccess = 'signature_saved_success';
  static const String signatureNotComplete = 'signature_not_complete';
  static const String signatureError = 'signature_error';
  static const String updateAccountSuccess = 'update_account_success';
  static const String updateAccountFailed = 'update_account_failed';
  static const String invalidIdCard = 'invalid_id_card';
  static const String cancelRegisterSuccess = 'cancel_register_success';
  static const String cancelRegisterFailed = 'cancel_register_failed';
  static const String cancelBuyBloodSuccess = 'cancel_buy_blood_success';
  static const String cancelBuyBloodFailed = 'cancel_buy_blood_failed';
  static const String updateInventorySuccess = 'update_inventory_success';
  static const String errorOccurredPleaseRetry = 'error_occurred_please_retry';
  static const String sendOtpSuccess = 'send_otp_success';
  static const String otpIncorrect = 'otpIncorrect';
  static const String otpSendSuccess = 'otpSendSuccess';
  static const String time = 'time';
  static const String invalidUsername = 'invalid_username';
  static const String scanIdCardToGetInfo = 'scan_id_card_to_get_info';
  static const String publishedDate = 'publishedDate';
  static const String fieldRequired = 'fieldRequired';
  static const String qrCodeRegistration = 'qrCodeRegistration';
  static const String eventId = 'eventId';
  static const String registrationId = 'registrationId';
  static const String registeringToDonate = 'registering_to_donate';
  // Question Answer - Personality Group Titles
  static const String qaDonationLocation = 'qaDonationLocation';
  static const String qaDonationTime = 'qaDonationTime';
  static const String qaDonationConditions = 'qaDonationConditions';
  static const String qaDonationNotes = 'qaDonationNotes';
  static const String qaDonationHandbook = 'qaDonationHandbook';
  static const String qaDonationBenefits = 'qaDonationBenefits';
  static const String qaBloodTesting = 'qaBloodTesting';
  static const String qaWhyNeedIdCard = 'qaWhyNeedIdCard';
  static const String qaCanGetInfected = 'qaCanGetInfected';
  static const String qaFeelUnwell = 'qaFeelUnwell';
  static const String qaSwellingSigns = 'qaSwellingSigns';
  static const String qaPrepareForDonation = 'qaPrepareForDonation';
  static const String qaAbnormalSituation = 'qaAbnormalSituation';
  static const String qaWhyNeedBlood = 'qaWhyNeedBlood';
  static const String qaIsDonationHarmful = 'qaIsDonationHarmful';
  static const String qaPostponeDonation = 'qaPostponeDonation';
  // Question Answer - Content
  static const String qaBloodTransfusionCenter = 'qaBloodTransfusionCenter';
  static const String qaMorning = 'qaMorning';
  static const String qaAfternoon = 'qaAfternoon';
  static const String qaAge = 'qaAge';
  static const String qaWeight = 'qaWeight';
  static const String qaDistanceBetweenDonations = 'qaDistanceBetweenDonations';
  static const String qaDistanceBetweenPlatelets = 'qaDistanceBetweenPlatelets';
  static const String qaDiseases = 'qaDiseases';
  static const String qaMenstrualCycle = 'qaMenstrualCycle';
  static const String qaForFemale = 'qaForFemale';
  static const String qaEatNormally = 'qaEatNormally';
  static const String qaBringIdCard = 'qaBringIdCard';
  static const String qaParkingLocation = 'qaParkingLocation';
  static const String qaBenefitsContent = 'qaBenefitsContent';
  static const String qaTestingContent = 'qaTestingContent';
  static const String qaTestingResult = 'qaTestingResult';
  static const String qaIdCardReason = 'qaIdCardReason';
  static const String qaCannotGetInfected = 'qaCannotGetInfected';
  static const String qaUnwellSymptoms = 'qaUnwellSymptoms';
  static const String qaSwellingInstructions = 'qaSwellingInstructions';
  static const String qaPrepareInstructions = 'qaPrepareInstructions';
  static const String qaAbnormalInstructions = 'qaAbnormalInstructions';
  static const String qaWhyNeedBloodReason = 'qaWhyNeedBloodReason';
  static const String qaHealthContent = 'qaHealthContent';
  static const String qaPostponeContent = 'qaPostponeContent';
  static const String qaDonateBlood = 'qaDonateBlood';
  static const String qaDonatePlatelets = 'qaDonatePlatelets';
  static const String qaWeekendDonation = 'qaWeekendDonation';
  static const String qaFromMondayToFriday = 'qaFromMondayToFriday';
  static const String qaSecondSunday = 'qaSecondSunday';
  static const String qaMayStopEarly = 'qaMayStopEarly';
  static const String qaExtension = 'qaExtension';
  static const String qaOr = 'qaOr';
  static const String qaContactBusinessHours = 'qaContactBusinessHours';
  static const String platelets = 'platelets';
  static const String blood = 'blood';
  static const String at = 'at';
  static const String timeFrom = 'time_from';
  static const String day = 'day';
  static const String noDataFromToDate = 'no_data_from_to_date';
  static const String updateIdCard = 'update_id_card';
  static const String here = 'here';
  static const String toViewDonationRegisterHistory =
      'to_view_donation_register_history';
  static const String toViewDonationHistory = 'to_view_donation_history';
  static const String apply = 'apply';
  static const String contactInformation = 'contact_information';
  static const String contactViaFanpage = 'contact_via_fanpage';
  static const String sendRequestDirectly = 'send_request_directly';
  static const String selectTimeRange = 'select_time_range';
  static const String forgotPassword = 'forgot_password';
  static const String pleaseFillInfoBelow = 'please_fill_info_below';
  static const String send = 'send';
  static const String address = 'address';
  static const String phone = 'phone';
  static const String extension = 'extension';
  static const String or = 'or';
  static const String contactDuringBusinessHours =
      'contact_during_business_hours';
  static const String version = 'version';
  static const String updateDate = 'update_date';
  static const String thankYouForUsingApp = 'thank_you_for_using_app';
  static const String rateApp = 'rate_app';
  // Biometric
  static const String loginWithBiometric = 'loginWithBiometric';
  static const String biometricNotAvailable = 'biometricNotAvailable';
  static const String biometricNotEnrolled = 'biometricNotEnrolled';
  static const String biometricAuthFailed = 'biometricAuthFailed';
  static const String biometricAuthSuccess = 'biometricAuthSuccess';
  static const String enableBiometricLogin = 'enableBiometricLogin';
  static const String biometricAuthReason = 'biometricAuthReason';
  static const String biometricAuth = 'biometricAuth';
  static const String emulatorMode = 'emulatorMode';
  static const String biometricDeviceNotSupported =
      'biometricDeviceNotSupported';
  static const String biometricNotLoggedIn = 'biometricNotLoggedIn';
  static const String biometricSessionExpired = 'biometricSessionExpired';
  static const String biometricLoginWithAccount = 'biometricLoginWithAccount';
  static const String biometricAuthNotFound = 'biometricAuthNotFound';
  static const String biometricTokenInvalid = 'biometricTokenInvalid';
  static const String biometricError = 'biometricError';
  static const String biometricLockedToOtherAccount =
      'biometricLockedToOtherAccount';
  static const String loginUsernameRequired = 'loginUsernameRequired';
  static const String loginPasswordRequired = 'loginPasswordRequired';
  static const String registerFullNameRequired = 'registerFullNameRequired';
  static const String registerFullNameInvalid = 'registerFullNameInvalid';
  static const String registerUsernameRequired = 'registerUsernameRequired';
  static const String registerPasswordRequired = 'registerPasswordRequired';
  static const String registerPasswordMinLength = 'registerPasswordMinLength';
  static const String registerPasswordNotMatch = 'registerPasswordNotMatch';
  static const String registerFullNameAccountRequired =
      'registerFullNameAccountRequired';
  static const String registerPhoneRequired = 'registerPhoneRequired';
  static const String registerPhoneInvalid = 'registerPhoneInvalid';
  static const String registerSuccess = 'registerSuccess';
  static const String registerFailed = 'registerFailed';
  static const String alreadyRegisteredSameDay = 'alreadyRegisteredSameDay';
  static const String notEnoughDaysToDonate = 'notEnoughDaysToDonate';
  static const String registerDonateBloodSuccess = 'registerDonateBloodSuccess';
  static const String scanQRSuccess = 'scanQRSuccess';
  static const String scanQRError = 'scanQRError';
  static const String changePasswordOldPasswordRequired =
      'changePasswordOldPasswordRequired';
  static const String forgotPasswordFullNameRequired =
      'forgotPasswordFullNameRequired';
  static const String forgotPasswordIDCardRequired =
      'forgotPasswordIDCardRequired';
  static const String forgotPasswordPhoneRequired =
      'forgotPasswordPhoneRequired';
  static const String forgotPasswordPhoneInvalidFormat =
      'forgotPasswordPhoneInvalidFormat';
  static const String forgotPasswordInfoSent = 'forgotPasswordInfoSent';
  static const String forgotPasswordSendError = 'forgotPasswordSendError';
  static const String homeDaysSinceLastDonation = 'homeDaysSinceLastDonation';
  static const String homeLastDonationDate = 'homeLastDonationDate';
  static const String homeWelcomeBack = 'homeWelcomeBack';
  static const String homeNoDonationYet = 'homeNoDonationYet';
  static const String profileIDCardInvalidFormat = 'profileIDCardInvalidFormat';
  static const String profilePhoneInvalidFormat = 'profilePhoneInvalidFormat';
  static const String profileDataUpdatedFromIDCard =
      'profileDataUpdatedFromIDCard';
  static const String profileBuyBloodAccountNote = 'profileBuyBloodAccountNote';
  static const String feedbackSendSuccess = 'feedbackSendSuccess';
  static const String feedbackPleaseUpdateInfoBeforeSend =
      'feedbackPleaseUpdateInfoBeforeSend';
  static const String registerBuyBloodNoInventoryToday =
      'registerBuyBloodNoInventoryToday';
  static const String registerBuyBloodQuantityExceeded =
      'registerBuyBloodQuantityExceeded';
  static const String registerBuyBloodPleaseSelectUnit =
      'registerBuyBloodPleaseSelectUnit';
  static const String routeNotFound = 'routeNotFound';
  static const String donationScheduleTestResultReactive =
      'donationScheduleTestResultReactive';
  static const String donationScheduleConfirmDonate =
      'donationScheduleConfirmDonate';
  static const String donationScheduleConfirmDonateMessage =
      'donationScheduleConfirmDonateMessage';
  static const String managementNoInventoryToday = 'managementNoInventoryToday';
  static const String managementErrorGettingInventory =
      'managementErrorGettingInventory';
  static const String managementNotification = 'managementNotification';
  static const String managementPleaseEnterApproveQuantity =
      'managementPleaseEnterApproveQuantity';
  static const String managementApproveBuyBloodSuccess =
      'managementApproveBuyBloodSuccess';
  static const String managementRejectBuyBloodSuccess =
      'managementRejectBuyBloodSuccess';
  static const String managementConfirmRejectRequest =
      'managementConfirmRejectRequest';
  static const String historyCancelBuyBloodSuccess =
      'historyCancelBuyBloodSuccess';
  static const String historyCancelBuyBloodFailed =
      'historyCancelBuyBloodFailed';
  static const String historyConfirmCancelBuyBlood =
      'historyConfirmCancelBuyBlood';
  static const String bloodTransfusionCenter = 'bloodTransfusionCenter';
  static const String bloodTransfusionCenterAddress =
      'bloodTransfusionCenterAddress';
  static const String exportQRCode = 'exportQRCode';
  static const String cancelRegistration = 'cancelRegistration';
  static const String status = 'status';
  static const String noDataFromDateToDate = 'noDataFromDateToDate';
  static const String cancelRegistrationSuccess = 'cancelRegistrationSuccess';
  static const String receivedAt = 'receivedAt';
  static const String processedAt = 'processedAt';
  static const String timeWithIndex = 'timeWithIndex';
  static const String selectImageFromGallery = 'selectImageFromGallery';
  static const String noQRCodeFoundInImage = 'noQRCodeFoundInImage';
  static const String failedToReadQRFromImage = 'failedToReadQRFromImage';
  // Digital signature flow
  static const String receptionStep = 'reception_step';
  static const String receptionStepTitle = 'reception_step_title';
  static const String receptionStepDescription = 'reception_step_description';
  static const String chooseSigningMethod = 'choose_signing_method';
  static const String handSignature = 'hand_signature';
  static const String digitalSignatureSmartCA = 'digital_signature_smartca';
  static const String pleaseSignYourNameBelow = 'please_sign_your_name_below';
  static const String signWithSmartCA = 'sign_with_smartca';
  static const String pleaseRegisterBeforeSigning =
      'please_register_before_signing';
  static const String digitalSignatureDescription =
      'digital_signature_description';
  static const String digitalSignatureFailed = 'digital_signature_failed';
  static const String goToSignatureScreen = 'go_to_signature_screen';
  static const String donorSignature = 'donor_signature';
  static const String measureVitalSigns = 'measure_vital_signs';
  static const String measureVitalSignsTitle = 'measure_vital_signs_title';
  static const String bloodPressure = 'blood_pressure';
  static const String heartRate = 'heart_rate';
  static const String temperature = 'temperature';
  static const String systolic = 'systolic';
  static const String diastolic = 'diastolic';
  static const String bpm = 'bpm';
  static const String celsius = 'celsius';
  static const String preDonationTest = 'pre_donation_test';
  static const String preDonationTestTitle = 'pre_donation_test_title';
  static const String staffSignature = 'staff_signature';
  static const String doctorConfirmation = 'doctor_confirmation';
  static const String doctorConfirmationTitle = 'doctor_confirmation_title';
  static const String doctorSignature = 'doctor_signature';
  static const String nurseBloodDraw = 'nurse_blood_draw';
  static const String nurseBloodDrawTitle = 'nurse_blood_draw_title';
  static const String nurseSignature = 'nurse_signature';
  static const String completeBloodDonation = 'complete_blood_donation';
  static const String thankYouLetterSent = 'thank_you_letter_sent';
  static const String pleaseEnterVitalSigns = 'please_enter_vital_signs';
  static const String pleaseSignToContinue = 'please_sign_to_continue';
  static const String stepCompleted = 'step_completed';
  static const String stepFailed = 'step_failed';
  static const String updateStatusSuccess = 'update_status_success';
  static const String updateStatusFailed = 'update_status_failed';
  static const String sendThankYouLetterSuccess =
      'send_thank_you_letter_success';
  static const String sendThankYouLetterFailed = 'send_thank_you_letter_failed';
  static const String bloodDonationCompleted = 'blood_donation_completed';
  static const String updateSignature = 'update_signature';
  static const String clear = 'clear';
  static const String pleaseSignAsStaff = 'please_sign_as_staff';
  static const String pleaseSignAsDoctor = 'please_sign_as_doctor';
  static const String pleaseSignAsNurse = 'please_sign_as_nurse';
  static const String feedbackTitle = 'feedback_title';
  static const String feedbackIntroMessage = 'feedback_intro_message';
  static const String feedbackEmailInvalid = 'feedback_email_invalid';
  static const String feedbackContentPlaceholder =
      'feedback_content_placeholder';

  static const Map<String, dynamic> EN = {
    homeBook: 'Book appointment',
    homeRegister: 'Register',
    homeHistory: 'History',
    homeLocation: "Location",
    homeBloodUsers: "Blood donation\nlist",
    homeStatictics: "Statistics",
    homeManagement: "Management",
    search: "Search",
    settings: "Settings",
    generalSetting: 'General setting',
    updateInformation: 'Update information',
    bloodDonationHistory: 'Blood donation history',
    bloodDonationRegister: 'Blood donation register',
    friend: 'Friend',
    listFriend: 'Friends',
    group: 'Group',
    joinGroup: 'Join group',
    deleteAccount: 'Delete account',
    logout: 'Logout',
    myAccount: 'My account',
    restorePurchased: 'restore_purchased',
    enableNotifications: 'Enable notifications',
    all_premium_access: 'All premium access',
    supportFeedback: 'Support & feedback',
    howToUseGuide: 'How to use guide',
    rateUs: 'Rate us',
    feedbackSupport: 'Feedback/support',
    moreAboutUs: 'More about us',
    aboutUs: 'About us',
    share: 'Share',
    privacyPolicy: 'Privacy policy',
    bloodType: 'Blood type',
    times: 'Times',
    filter: 'Filter',
    provinceCity: 'Province/City',
    enterProvinceCity: 'Enter Province/City',
    district: 'District',
    enterDistrict: 'Enter District',
    ward: 'Ward',
    fromDate: 'From date',
    today: 'Today',
    tomorrow: 'Tomorrow',
    enterTime: 'Enter time',
    toDay: 'To day',
    location: 'Location',
    application: 'Application',
    news: 'news',
    all: 'all',
    dontHaveAnAccount: "Don't have an account yet?",
    loginByOtp: "Login by number phone",
    login: 'Login',
    contact: 'Contact',
    signUp: 'Sign Up',
    username: 'Username or Number Phone',
    password: 'Password',
    oldPassword: 'Old password',
    confirmPassword: 'Confirm password',
    loginWithGoogle: 'Login with Google',
    loginFail: 'Wrong username or password',
    authLoginWith: 'Or',
    formRequiredPasswordError: 'Please enter your password',
    formRequiredUsernameError: 'Please enter your username',
    formRequiredNameError: 'Please enter your name',
    formRequiredPhoneError: 'Please enter your phone',
    road: 'road',
    register: 'Register',
    registerDonateBlood: 'Register to donate blood',
    next: 'next',
    prev: 'prev',
    name: 'Name',
    gender: 'Gender',
    yearOfBirth: 'Year of birth',
    male: 'Male',
    female: 'Female',
    expectedArrivalDate: 'Expected arrival date',
    occupation: 'Occupation',
    company: 'Company',
    registerBuyBlood: 'Register buy blood',
    approveBuyBlood: 'Approve buy blood',
    bloodDonationSchedule: 'Blood donation schedule',
    fullname: 'Fullname',
    birthYear: 'Birth year',
    dateOfBirth: 'Date of birth',
    idCard: 'ID card',
    phoneNumber: 'Phone number',
    email: 'Email',
    notification: 'Notification',
    notificationUpdateProfile: 'Would you like to update your information?',
    cancel: 'Cancel',
    accept: 'Accept',
    yes: 'Yes',
    no: 'No',
    dominance: 'Dominance',
    influence: 'Influence',
    steadiness: 'Steadiness',
    compliance: 'Compliance',
    personalityGroup: 'Personality group',
    questionAndAnswer: 'Question - answer',
    listRegister: 'List register',
    selectBloodBuyUnit: 'Select the unit for blood concession',
    note: 'Note',
    accountInformation: 'Account information',
    changePassword: 'Change password',
    appInformation: 'App information',
    versionInformation: 'Version information',
    setup: 'Setup',
    language: 'Language',
    english: 'English',
    vietnamese: 'Vietnamese',
    thankYouMessage:
        'Sincere thanks to Medcomtech Company for accompanying Cho Ray Blood Transfusion Center in voluntary blood donation activities.',
    confirmDeleteAccount: 'Confirm delete account',
    deleteAccountMessage: 'Your account will be deleted\npermanently',
    confirmLogout: 'Confirm logout',
    logoutMessage: 'Are you sure you want to logout?',
    confirmExitRegisterDonateBlood: 'Confirm',
    confirmExitRegisterDonateBloodMessage:
        'Confirm exit blood donation registration screen',
    confirmRegisterWithYesAnswer: 'Confirm',
    confirmRegisterWithYesAnswerMessage:
        'One (or more) answers are selected as "Yes"\r\nDo you want to register?',
    logoutFailed: 'Logout failed. Please try again.',
    deleteAccountSuccess: 'Account deleted successfully',
    deleteAccountFailed: 'Failed to delete account',
    scanQRCCCD: 'Scan QR CCCD/ID Card',
    tryAgain: 'Try again',
    back: 'Back',
    close: 'Close',
    cameraError: 'Camera error',
    cameraInitError: 'Camera initialization error',
    qrScanInstruction: 'Place the QR code on your ID card in the frame',
    cannotInitCamera:
        'Cannot initialize camera. Please rebuild the app and grant camera permission.\nError:',
    error: 'Error',
    cannotReadQRCode: 'Cannot read information from QR code:',
    passwordNotMatch: 'Password and confirm password do not match',
    registerAccountSuccess: 'Account registration successful!',
    registerAccountFailed: 'Account registration failed!',
    pleaseSignBeforeConfirm: 'Please sign before confirming',
    confirm: 'Confirm',
    registerAccount: 'Register account',
    signatureConfirmed: 'Digital signature confirmed!',
    passwordMinLength: 'Password must be at least 6 characters',
    cannotSaveSignature: 'Cannot save signature',
    errorOccurred: 'An error occurred:',
    signatureCancelled: 'Digital signature cancelled',
    functions: 'Functions',
    home: 'Home',
    personal: 'Personal',
    historyRegisterBuyBlood: 'Registration history\nfor blood concession',
    approveBuyBloodRequest: 'Approve blood\nconcession request',
    contactSupport: 'Contact\nsupport',
    registerBuyBloodShort: 'Register\nblood concession',
    bloodDonationScheduleShort: 'Blood donation\nschedule',
    bloodDonationHistoryShort: 'Blood donation\nhistory',
    bloodDonationRegisterShort: 'Registration history\nfor blood donation',
    yourAccount: 'Your account',
    softwareInformation: 'Software information',
    noNotificationYet: "You don't have any notifications yet",
    notUpdatePersonalInfo: "You have not updated personal information.",
    updateNow: "Update now",
    noData: 'No data',
    pleaseUpdatePersonalInfoBefore: 'Please update personal information before',
    expectedArrivalDateTime: 'Expected arrival date and time',
    contactAddress: 'Contact address',
    fromToDate: 'From {fromDate} to {toDate}',
    pleaseEnterApprovalQuantity: 'Please enter approval quantity',
    pleaseSelect: 'Please select',
    pleaseEnter: 'Please enter',
    pleaseWait: 'Please wait',
    pleaseUpdateAppVersion: 'Please update to the new version!',
    newVersionAvailable: 'New version available',
    pleaseEnterFullInfo: 'Please enter full information\nto continue!',
    pleaseEnterFullNameBeforeSign: 'Please enter full name before signing',
    pleaseEnterIdCardBeforeSign: 'Please enter ID card before signing',
    pleaseEnterFeedbackContent: 'Please enter feedback content',
    pleaseUpdatePersonalInfoBeforeFeedback:
        'Please update personal information before sending feedback!',
    pleaseUpdatePersonalInfoBeforeRegister:
        'Please update personal information before registering to donate blood!',
    pleaseUpdatePersonalInfoBeforeBuyBlood:
        'Please update personal information before creating blood concession request!',
    saveChanges: 'Save changes',
    selectDateTime: 'Select date and time',
    selectBloodDonationSchedule: 'Select blood donation schedule',
    // Question Answer - Titles
    qaDonationLocation: 'WHERE IS THE BLOOD DONATION LOCATION?',
    qaDonationTime: 'WHAT IS THE BLOOD DONATION REGISTRATION TIME?',
    qaDonationConditions:
        'WHAT ARE THE STANDARDS FOR BLOOD AND PLATELET DONATION?',
    qaDonationNotes: 'WHAT TO NOTE WHEN COMING TO DONATE BLOOD?',
    qaDonationHandbook: 'HANDBOOK',
    qaDonationBenefits: 'WHAT ARE THE BENEFITS OF VOLUNTARY BLOOD DONATION?',
    qaBloodTesting: 'WHAT TESTS WILL MY BLOOD UNDERGO?',
    qaWhyNeedIdCard: 'WHY DO I NEED ID CARD FOR BLOOD DONATION?',
    qaCanGetInfected: 'CAN I GET INFECTED WHEN DONATING BLOOD?',
    qaFeelUnwell: 'FEELING UNWELL AFTER DONATING BLOOD?',
    qaSwellingSigns: 'HAVING SWELLING SIGNS AT THE INJECTION SITE?',
    qaPrepareForDonation: 'I WILL DONATE BLOOD TOMORROW, HOW SHOULD I PREPARE?',
    qaAbnormalSituation:
        'WHEN DETECTING ABNORMALITIES, FEELING UNSAFE WITH THE BLOOD BAG JUST DONATED?',
    qaWhyNeedBlood: 'WHY DO SO MANY PEOPLE NEED BLOOD TRANSFUSION?',
    qaIsDonationHarmful: 'IS HUMANITARIAN BLOOD DONATION HARMFUL TO HEALTH?',
    qaPostponeDonation: 'WHAT CASES NEED TO POSTPONE BLOOD DONATION?',
    // Question Answer - Content
    qaBloodTransfusionCenter: 'CHO RAY BLOOD TRANSFUSION CENTER',
    qaMorning: 'Morning',
    qaAfternoon: 'Afternoon',
    qaAge: 'Age',
    qaWeight: 'Weight',
    qaDistanceBetweenDonations: 'Distance between 2 blood donations',
    qaDistanceBetweenPlatelets: 'Distance between 2 platelet donations',
    qaDiseases: 'Diseases',
    qaMenstrualCycle: 'End of menstrual cycle',
    qaForFemale: '(for females)',
    qaEatNormally: 'Eat normally and',
    qaBringIdCard: 'When coming to donate blood, please bring your ID card.',
    qaParkingLocation:
        'Blood donors park at Gate 3 - Cho Ray Hospital, Thuan Kieu Street.',
    qaBenefitsContent:
        'Rights and benefits for voluntary blood donors according to Circular No. 05/2017/TT-BYT...',
    qaTestingContent:
        'All blood units collected will be tested for blood type (ABO system, Rh system), HIV, hepatitis B virus, hepatitis C virus, syphilis, malaria.',
    qaTestingResult:
        'You will be notified of results, kept confidential and consulted (free of charge) when the above infectious diseases are detected.',
    qaIdCardReason:
        'Each blood unit must have a record containing information about the blood donor. According to regulations, this is a necessary procedure in the blood donation process to ensure the authenticity of information about the blood donor.',
    qaCannotGetInfected:
        'The blood collection needle is sterile, used only once per person, so it cannot transmit diseases to the blood donor.',
    qaUnwellSymptoms:
        'After donating blood, if you have symptoms such as dizziness, fatigue, nausea, etc., please contact the blood collection unit immediately for medical support.',
    qaSwellingInstructions:
        'After donating blood, if you have signs of swelling, edema at the injection site. Please do not worry too much, apply cold compress immediately to the swollen area and monitor the signs above. If it does not decrease after 24 hours, please contact the blood collection unit again for support.',
    qaPrepareInstructions:
        '• Tonight you should not stay up too late (sleep before 23:00).\n• Should eat and not drink alcohol, beer before donating blood.\n• Bring ID card, sufficient personal documents and blood donation card (if any) when going to donate blood.',
    qaAbnormalInstructions:
        'After participating in blood donation, if you discover anything that makes you feel unsafe with the blood bag just donated (suddenly remember a risky behavior, have used any medication that you forgot to report to the doctor during examination, have tested POSITIVE for SARS-CoV-2 using rapid test or Real time RT-PCR technique, etc.) please report back to the blood collection unit where you participated in donation.',
    qaWhyNeedBloodReason:
        'Every hour hundreds of patients need to receive blood transfusion because:\n• Blood loss due to trauma, accidents, disasters, gastrointestinal bleeding...\n• Due to diseases causing anemia, bleeding: blood cancer, bone marrow failure, hemophilia...\n• Modern treatment methods require a lot of blood: cardiovascular surgery, organ transplantation...',
    qaHealthContent:
        'Blood donation according to medical guidance is not harmful to health. This has been proven by scientific and practical evidence:\n• Blood has many components, each component has a certain lifespan and is constantly renewed daily. For example: Red blood cells live for 120 days, plasma is constantly replaced and renewed. Scientific evidence shows that if each donation is less than 1/10 of the blood volume in the body, it is not harmful to health.\n• Many research projects have proven that after donating blood, blood indicators change slightly but still within normal physiological limits, not affecting the daily activities of the body.\n• In practice, millions of people have donated blood many times and their health is still completely good. In the world, there are people who have donated blood more than 400 times. In Vietnam, the person who has donated blood the most times has donated nearly 100 times, and their health is completely good.\n• Thus, each person if they feel good health, do not have infectious diseases transmitted through blood, meet blood donation standards, can donate blood 3-4 times a year, both not adversely affecting their own health, and ensuring good quality blood, safe for patients.',
    qaPostponeContent:
        '• People who must postpone blood donation for 12 months from the time:\n   + Fully recovered after surgical interventions.\n   + Recovered from one of the diseases: malaria, syphilis, tuberculosis, tetanus, encephalitis, meningitis.\n   + End of rabies vaccination after being bitten by animals or injection, blood transfusion, blood products and biological products derived from blood.\n   + Childbirth or termination of pregnancy.\n\n• People who must postpone blood donation for 06 months from the time:\n   + Tattooing on the skin.\n   + Ear piercing, nose piercing, navel piercing or other parts of the body.\n   + Exposure to blood and body fluids from people at risk or infected with blood-borne diseases.\n   + Recovered from one of the diseases: typhoid, septicemia, snake bite, arterial occlusion, venous occlusion, osteomyelitis, pancreatitis.\n\n• People who must postpone blood donation for 04 weeks from the time:\n   + Recovered from one of the diseases: gastroenteritis, urinary tract infection, skin infection, bronchitis, pneumonia, measles, whooping cough, mumps, dengue fever, dysentery, rubella, cholera, mumps.\n   + End of vaccination for rubella, measles, typhoid, cholera, mumps, chickenpox, BCG.\n\n• People who must postpone blood donation for 07 days from the time:\n   + Recovered from one of the diseases: flu, cold, nasal allergy, pharyngitis, migraine headache.\n   + Vaccination with various vaccines, except those specified in Point c Clause 1 and Point b Clause 3 of this Article.\n\n• Some regulations related to occupation and special activities of blood donors: people doing certain jobs and performing the following special activities can only donate blood on days off or can only perform these jobs and activities at least 12 hours after donating blood:\n   + People working at height or depth: pilots, crane operators, workers working at height, mountaineers, miners, sailors, divers.\n   + People operating public transport vehicles: bus drivers, train drivers, ship drivers.\n   + Other cases: professional athletes, people doing heavy exercise, heavy training.',
    qaDonateBlood: 'Blood donation',
    qaDonatePlatelets: 'Platelet donation',
    qaWeekendDonation: 'Weekend blood donation',
    qaFromMondayToFriday: 'from Monday to Friday weekly',
    qaSecondSunday: 'Second Sunday of each month',
    qaMayStopEarly:
        '(May stop accepting registrations earlier when demand is met)',
    notEnterOldPassword: 'Please enter old password',
    notEnterPassword: 'Please enter password',
    changePasswordSuccess: 'Password changed successfully',
    changePasswordFailed: 'Password change failed',
    numberOfBloodDonations: 'Number of blood donations',
    welcomeBack: 'Welcome back!',
    notDonatedYet: "You haven't donated blood yet!",
    daysSinceLastDonation:
        "It's been {days} days since you last donated blood!\n",
    lastDonation: 'Your last blood donation: {date}',
    notificationTitle: 'Notification',
    cannotFindRoute: 'Cannot find route',
    pleaseSelectBloodUnit: 'Please select blood unit',
    approveBuyBloodSuccess: 'Blood concession request approved successfully',
    rejectBuyBloodSuccess: 'Blood concession request rejected successfully',
    sendSuccess: 'Sent successfully!',
    notEnterFullName: 'Please enter full name',
    invalidFullName: 'Invalid full name',
    notEnterUsername: 'Please enter username',
    notEnterPhone: 'Please enter phone number',
    invalidPhone: 'Invalid phone number',
    qrCodeReadSuccess: 'QR code information retrieved successfully!',
    qrCodeReadError: 'Error reading QR code information!',
    qrScanError: 'Error scanning QR code!',
    signatureSavedSuccess: 'Signature saved successfully!',
    signatureNotComplete: 'Signature not completed',
    signatureError: 'Error performing digital signature:',
    updateAccountSuccess: 'Account updated successfully',
    updateAccountFailed: 'Account update failed',
    invalidIdCard: 'ID card format is incorrect!',
    cancelRegisterSuccess: 'Registration cancelled successfully',
    cancelRegisterFailed: 'Registration cancellation failed',
    cancelBuyBloodSuccess: 'Blood concession request cancelled successfully',
    cancelBuyBloodFailed: 'Blood concession request cancellation failed',
    updateInventorySuccess: 'Inventory information updated successfully',
    errorOccurredPleaseRetry: 'An error occurred, please try again later',
    sendOtpSuccess: 'OTP sent successfully',
    otpIncorrect: 'OTP code is incorrect',
    otpSendSuccess: 'OTP sent successfully',
    time: 'Time',
    invalidUsername: 'Username must be ID card with 9 or 12 characters!',
    usernameWithIdCard: 'ID Card',
    scanIdCardToGetInfo: 'Scan ID card to get information',
    publishedDate: 'Published date',
    fieldRequired: 'This field is required',
    qrCodeRegistration: 'QR code registration for blood donation',
    eventId: 'Event ID',
    registrationId: 'Registration ID',
    registeringToDonate: 'You are registering to donate',
    youAreRegisteringToDonate: 'You are registering to donate {type}',
    registeredCount: 'Registered: {registered}/{total}',
    platelets: 'platelets',
    blood: 'blood',
    at: 'at',
    timeFrom: 'Time from',
    timeFromDate: 'Time from {time} on {date}',
    day: 'day',
    noDataFromToDate: 'No data\nfrom {fromDate} to {toDate}',
    updateIdCard: 'Update ID card',
    here: 'Here',
    toViewDonationRegisterHistory:
        ' to view your blood donation registration history.',
    toViewDonationHistory: ' to view your blood donation history.',
    apply: 'Apply',
    selectTimeRange: 'Select time range',
    contactInformation: 'CONTACT INFORMATION',
    contactViaFanpage: 'CONTACT VIA FANPAGE',
    sendRequestDirectly: 'SEND REQUEST DIRECTLY',
    forgotPassword: 'Did you forget your password?',
    pleaseFillInfoBelow:
        'Please fill in the information below.\nThe center will contact you to reset your password!',
    send: 'Send',
    address: 'Address: ',
    phone: 'Phone: ',
    extension: ' - extension ',
    or: ' or ',
    contactDuringBusinessHours: ' (contact during business hours).',
    version: 'Version: V.',
    updateDate: 'Update date: ',
    thankYouForUsingApp:
        'Thank you for using our app!\nPlease leave a review to help us improve',
    rateApp: 'RATE APP',
    // Biometric
    loginWithBiometric: 'Login with fingerprint/Face ID',
    biometricNotAvailable:
        'Biometric authentication is not available on this device',
    biometricNotEnrolled: 'No biometric credentials found. Please login first.',
    biometricAuthFailed: 'Biometric authentication failed',
    biometricAuthSuccess: 'Login successful',
    enableBiometricLogin: 'Enable biometric login',
    biometricAuthReason: 'Please authenticate to login',
    biometricAuth: 'Biometric Authentication',
    emulatorMode: 'Emulator Mode - Simulating biometric authentication',
    biometricDeviceNotSupported:
        'Your device does not support fingerprint/Face ID. Please login with username and password.',
    biometricNotLoggedIn:
        'You have not logged in. Please login with username and password before using fingerprint/Face ID login.',
    biometricSessionExpired:
        'Your login session has expired. Please login again with username and password.',
    biometricLoginWithAccount: 'Login with account: {account}\n\n{reason}',
    biometricAuthNotFound: 'Login information not found. Please login again.',
    biometricTokenInvalid: 'Invalid token. Please login again.',
    biometricError: 'Error: {error}',
    biometricLockedToOtherAccount:
        'Biometric login is already linked to another account. Please clear biometric login before switching accounts.',
    loginUsernameRequired: 'Please enter username or ID card',
    loginPasswordRequired: 'Please enter password',
    registerFullNameRequired: 'Please enter full name',
    registerFullNameInvalid: 'Invalid full name',
    registerUsernameRequired: 'Please enter username',
    registerPasswordRequired: 'Please enter password',
    registerPasswordMinLength: 'Password must be at least 6 characters',
    registerPasswordNotMatch: 'Password and confirm password do not match',
    registerFullNameAccountRequired: 'Please enter account full name',
    registerPhoneRequired: 'Please enter phone number',
    registerPhoneInvalid: 'Invalid phone number',
    registerSuccess: 'Account registration successful!',
    registerFailed: 'Account registration failed!',
    alreadyRegisteredSameDay:
        'You have already registered for a blood donation (different) today. Please check and cancel before registering a new one!',
    notEnoughDaysToDonate:
        'You have not met the required number of days ({days} days) to donate {type}.\nYour last {type} donation was on {date}',
    registerDonateBloodSuccess: 'Registration successful',
    scanQRSuccess: 'QR code scanned successfully!',
    scanQRError: 'Error reading QR code',
    changePasswordOldPasswordRequired: 'Please enter old password',
    forgotPasswordFullNameRequired: 'Please enter full name',
    forgotPasswordIDCardRequired: 'Please enter ID card',
    forgotPasswordPhoneRequired: 'Please enter phone number',
    forgotPasswordPhoneInvalidFormat: 'Invalid phone number format',
    forgotPasswordInfoSent: 'Information has been sent!',
    forgotPasswordSendError: 'Error sending information',
    homeDaysSinceLastDonation:
        'It has been {days} days since you last donated blood!\n',
    homeLastDonationDate: 'Your last blood donation: {date}',
    homeWelcomeBack: 'Welcome back!',
    homeNoDonationYet: "You haven't donated blood yet!",
    profileIDCardInvalidFormat: 'ID card format is incorrect!',
    profilePhoneInvalidFormat: 'Phone number format is incorrect!',
    profileDataUpdatedFromIDCard:
        'Data has been updated according to\nID card.\nIf you want to change, please contact\nCho Ray Blood Transfusion Center',
    profileBuyBloodAccountNote:
        'This is a blood concession registration account\nCannot edit information.',
    feedbackSendSuccess: 'Sent successfully!',
    feedbackPleaseUpdateInfoBeforeSend:
        'Please update personal information before sending feedback!',
    registerBuyBloodNoInventoryToday:
        'Currently no inventory on {date}, Please choose another date!',
    registerBuyBloodQuantityExceeded:
        'Quantity {bloodType} of {productType} ({quantity}) cannot be greater than ({available})',
    registerBuyBloodPleaseSelectUnit: 'Please select blood unit.',
    routeNotFound: 'Route not found',
    donationScheduleTestResultReactive:
        'Your latest test result is Reactive, so you cannot register to donate blood',
    donationScheduleConfirmDonate: 'Confirm donate {type}',
    donationScheduleConfirmDonateMessage:
        'Agree to register to donate {type}\rat this location?',
    managementNoInventoryToday:
        'Currently no inventory on {date}, please update inventory!',
    managementErrorGettingInventory:
        'Error getting inventory on {date}, please contact Technician!',
    managementNotification: 'Notification',
    managementPleaseEnterApproveQuantity: 'Please enter approve quantity',
    managementApproveBuyBloodSuccess:
        'Approve blood concession request successful.',
    managementRejectBuyBloodSuccess:
        'Reject blood concession request successful.',
    managementConfirmRejectRequest: 'Confirm reject this request',
    historyCancelBuyBloodSuccess: 'Cancel blood concession request successful',
    historyCancelBuyBloodFailed: 'Cancel blood concession request failed',
    historyConfirmCancelBuyBlood:
        'Confirm cancel this blood concession request?',
    exportQRCode: 'Export QR Code',
    cancelRegistration: 'Cancel Registration',
    status: 'Status',
    noDataFromDateToDate: 'No data from {fromDate} to {toDate}',
    cancelRegistrationSuccess: 'Cancel registration successful',
    receivedAt: 'Received at:',
    processedAt: 'Processed at:',
    timeWithIndex: 'Time {index}',
    selectImageFromGallery: 'Select image from gallery',
    noQRCodeFoundInImage: 'No QR code found in the selected image',
    failedToReadQRFromImage: 'Failed to read QR code from image',
    // Digital signature flow
    receptionStep: 'Reception',
    receptionStepTitle: 'Reception - Donor Signature',
    receptionStepDescription: 'Please sign your name to confirm reception',
    chooseSigningMethod: 'Choose signing method:',
    handSignature: 'Hand signature',
    digitalSignatureSmartCA: 'Digital signature SmartCA',
    pleaseSignYourNameBelow: 'Please sign your name in the box below',
    signWithSmartCA: 'Sign with SmartCA',
    pleaseRegisterBeforeSigning: 'Please register before signing.',
    digitalSignatureDescription:
        'Digital signature will be performed through the SmartCA system. You will be required to authenticate to complete the signing process.',
    digitalSignatureFailed: 'Digital signature failed',
    goToSignatureScreen: 'Go to signature screen',
    donorSignature: 'Donor Signature',
    measureVitalSigns: 'Measure Vital Signs',
    measureVitalSignsTitle: 'Measure Vital Signs',
    bloodPressure: 'Blood Pressure',
    heartRate: 'Heart Rate',
    temperature: 'Temperature',
    systolic: 'Systolic',
    diastolic: 'Diastolic',
    bpm: 'bpm',
    celsius: '°C',
    preDonationTest: 'Pre-donation Test',
    preDonationTestTitle: 'Pre-donation Test - Staff Signature',
    staffSignature: 'Staff Signature',
    doctorConfirmation: 'Doctor Confirmation',
    doctorConfirmationTitle: 'Doctor Confirmation - Doctor Signature',
    doctorSignature: 'Doctor Signature',
    nurseBloodDraw: 'Nurse Blood Draw',
    nurseBloodDrawTitle: 'Nurse Blood Draw - Nurse Signature',
    nurseSignature: 'Nurse Signature',
    completeBloodDonation: 'Complete Blood Donation',
    thankYouLetterSent: 'Thank you letter has been sent',
    pleaseEnterVitalSigns: 'Please enter all vital signs',
    pleaseSignToContinue: 'Please sign to continue',
    stepCompleted: 'Step completed successfully',
    stepFailed: 'Step failed. Please try again',
    updateStatusSuccess: 'Status updated successfully',
    updateStatusFailed: 'Failed to update status',
    sendThankYouLetterSuccess: 'Thank you letter sent successfully',
    sendThankYouLetterFailed: 'Failed to send thank you letter',
    bloodDonationCompleted: 'Blood donation completed successfully!',
    updateSignature: 'Update Signature',
    clear: 'Clear',
    pleaseSignAsStaff: 'Please sign as staff member',
    pleaseSignAsDoctor: 'Please sign as doctor',
    pleaseSignAsNurse: 'Please sign as nurse',
    feedbackTitle: 'Feedback',
    feedbackIntroMessage:
        'We appreciate your feedback. Please fill in the form below.',
    feedbackEmailInvalid: 'Please enter a valid email address',
    feedbackContentPlaceholder: 'Feedback content',
  };

  static const Map<String, dynamic> VI = {
    homeBook: 'Đặt lịch',
    homeRegister: 'Đăng ký',
    homeHistory: 'Lịch sử',
    homeLocation: "Địa điểm",
    homeBloodUsers: "Danh sách\nhiến máu",
    homeStatictics: "Thống kê",
    homeManagement: "Quản lý",
    search: "Tìm kiếm",
    settings: "Cài đặt",
    generalSetting: 'Cài đặt chung',
    updateInformation: 'Cập nhật thông tin',
    bloodDonationHistory: 'Lịch sử hiến máu',
    bloodDonationRegister: 'Lịch sử đăng ký hiến máu',
    friend: 'Bạn bè',
    listFriend: 'Danh sách bạn bè',
    group: 'Nhóm',
    joinGroup: 'Tham gia nhóm',
    deleteAccount: 'Xóa tài khoản',
    logout: 'Đăng xuất',
    myAccount: 'Tài khoản của tôi',
    enableNotifications: 'Hiển thị thông báo',
    all_premium_access: 'Tất cả quyền truy cập cấp cao',
    supportFeedback: 'Hỗ trợ và phản hồi',
    howToUseGuide: 'Hướng dẫn sử dụng',
    rateUs: 'Đánh giá',
    feedbackSupport: 'Liên hệ - hỗ trợ',
    moreAboutUs: 'Thông tin về chúng tôi',
    aboutUs: 'Về chúng tôi',
    share: 'Chia sẻ',
    privacyPolicy: 'Chính sách bảo mật',
    bloodType: 'Nhóm máu',
    times: 'Số lần',
    filter: 'Bộ lọc',
    enterTime: 'Nhập thời gian',
    provinceCity: 'Tỉnh/Thành phố',
    enterProvinceCity: 'Nhập Tỉnh/Thành phố',
    district: 'Huyện/Quận',
    enterDistrict: 'Nhập Huyện/Quận',
    ward: 'Phường/Xã',
    fromDate: 'Từ ngày',
    today: 'Hôm nay',
    tomorrow: 'Ngày mai',
    toDay: 'Đến ngày',
    location: 'Địa điểm',
    application: 'Ứng dụng',
    news: 'Tin tức',
    all: 'Tất cả',
    dontHaveAnAccount: 'Bạn chưa có tài khoản?',
    loginByOtp: 'Đăng nhập bằng số điện thoại',
    login: 'Đăng nhập',
    contact: 'Liên hệ',
    signUp: 'Đăng ký',
    username: 'Tên đăng nhập/CCCD/Căn cước',
    password: 'Mật khẩu',
    oldPassword: 'Mật khẩu cũ',
    confirmPassword: 'Nhập lại mật khẩu',
    loginWithGoogle: 'Đăng nhập với Google',
    loginFail: 'Sai tên đăng nhập hoặc mật khẩu',
    authLoginWith: 'Hoặc',
    formRequiredPasswordError: 'Vui lòng nhập mật khẩu',
    formRequiredUsernameError: 'Vui lòng nhập tài khoản',
    formRequiredNameError: 'Vui lòng nhập họ tên',
    formRequiredPhoneError: 'Vui lòng nhập số điện thoại',
    road: 'Đường đi',
    register: 'Đăng ký',
    registerDonateBlood: 'Đăng ký hiến máu',
    next: 'Tiếp theo',
    prev: 'Quay lại',
    name: 'Tên',
    gender: 'Giới tính',
    yearOfBirth: 'Năm sinh',
    male: 'Nam',
    female: 'Nữ',
    expectedArrivalDate: 'Ngày giờ dự kiến đến',
    occupation: 'Nghề nghiệp',
    company: 'Công ty',
    registerBuyBlood: 'Đăng ký nhượng máu',
    approveBuyBlood: 'Duyệt yêu cầu nhượng máu',
    bloodDonationSchedule: 'Lịch hiến máu',
    fullname: 'Họ và tên',
    birthYear: 'Năm sinh',
    dateOfBirth: 'Ngày sinh',
    idCard: 'CCCD/Căn cước',
    phoneNumber: 'Số điện thoại',
    email: 'Email',
    notification: 'Thông báo',
    notificationUpdateProfile: 'Bạn có muốn cập nhật thông tin không?',
    cancel: 'Hủy',
    accept: 'Đồng ý',
    yes: 'Có',
    no: 'Không',
    dominance: 'Sự thống trị',
    influence: 'Sự ảnh hưởng',
    steadiness: 'Sự ổn định',
    compliance: 'Sự tuân thủ',
    personalityGroup: 'Nhóm tính cách',
    questionAndAnswer: 'Hỏi - đáp',
    listRegister: 'Danh sách đăng ký',
    selectBloodBuyUnit: 'Chọn đơn vị cho nhượng máu',
    note: 'Ghi chú',
    accountInformation: 'Thông tin tài khoản',
    changePassword: 'Thay đổi mật khẩu',
    appInformation: 'Thông tin ứng dụng',
    versionInformation: 'Thông tin phiên bản',
    setup: 'Thiết lập',
    language: 'Ngôn ngữ',
    english: 'Tiếng Anh',
    vietnamese: 'Tiếng Việt',
    thankYouMessage:
        'Chân thành cảm ơn Công ty Medcomtech đã đồng hành cùng Trung tâm Truyền máu Chợ Rẫy trong hoạt động vận động hiến máu tình nguyện.',
    confirmDeleteAccount: 'Xác nhận xóa tài khoản',
    deleteAccountMessage: 'Tài khoản của bạn sẽ bị xóa\nvĩnh viễn',
    confirmLogout: 'Xác nhận đăng xuất',
    logoutMessage: 'Bạn có chắc chắn muốn đăng xuất?',
    confirmExitRegisterDonateBlood: 'Xác nhận',
    confirmExitRegisterDonateBloodMessage:
        'Xác nhận thoát màn hình đăng ký hiến máu',
    confirmRegisterWithYesAnswer: 'Xác nhận',
    confirmRegisterWithYesAnswerMessage:
        'Một (hoặc nhiều) câu trả lời đang chọn là \'Có\'\r\nBạn có muốn đăng ký ?',
    logoutFailed: 'Đăng xuất thất bại. Vui lòng thử lại.',
    deleteAccountSuccess: 'Xóa tài khoản thành công',
    deleteAccountFailed: 'Xóa tài khoản thất bại',
    scanQRCCCD: 'Quét mã QR CCCD/Căn cước',
    tryAgain: 'Thử lại',
    back: 'Quay lại',
    close: 'Đóng',
    cameraError: 'Lỗi camera',
    cameraInitError: 'Lỗi khởi tạo camera',
    qrScanInstruction: 'Đưa mã QR trên căn cước công dân vào khung',
    cannotInitCamera:
        'Không thể khởi tạo camera. Vui lòng rebuild app và cấp quyền camera.\nLỗi:',
    error: 'Lỗi',
    cannotReadQRCode: 'Không thể đọc thông tin từ QR code:',
    passwordNotMatch: 'Mật khẩu và xác nhận mật khẩu không giống nhau',
    registerAccountSuccess: 'Đăng ký tài khoản thành công!',
    registerAccountFailed: 'Đăng ký tài khoản thất bại!',
    pleaseSignBeforeConfirm: 'Vui lòng ký tên trước khi xác nhận',
    confirm: 'Xác nhận',
    registerAccount: 'Đăng ký tài khoản',
    signatureConfirmed: 'Đã xác nhận chữ ký số!',
    passwordMinLength: 'Mật khẩu phải từ 6 ký tự',
    cannotSaveSignature: 'Không thể lưu chữ ký',
    errorOccurred: 'Có lỗi xảy ra:',
    signatureCancelled: 'Đã hủy chữ ký số',
    functions: 'Chức năng',
    home: 'Trang chủ',
    personal: 'Cá nhân',
    historyRegisterBuyBlood: 'Lịch sử ĐK\nnhượng máu',
    approveBuyBloodRequest: 'Duyệt YC\nnhượng máu',
    contactSupport: 'Liên hệ\nhỗ trợ',
    registerBuyBloodShort: 'Đăng ký\nnhượng máu',
    bloodDonationScheduleShort: 'Lịch\nhiến máu',
    bloodDonationHistoryShort: 'Lịch sử\nhiến máu',
    bloodDonationRegisterShort: 'Lịch sử\nđ.ký hiến máu',
    yourAccount: 'Tài khoản của bạn',
    softwareInformation: 'Thông tin phần mềm',
    noNotificationYet: 'Bạn chưa có thông báo nào',
    notUpdatePersonalInfo: 'Bạn chưa cập nhật thông tin cá nhân.',
    updateNow: 'Cập nhật ngay',
    noData: 'Không có dữ liệu',
    pleaseUpdatePersonalInfoBefore:
        'Vui lòng cập nhật thông tin cá nhân trước khi',
    expectedArrivalDateTime: 'Ngày giờ dự kiến đến',
    pleaseSelect: 'Vui lòng chọn',
    pleaseEnter: 'Vui lòng nhập',
    pleaseWait: 'Vui lòng chờ',
    pleaseUpdateAppVersion: 'Vui lòng cập nhật version mới!',
    newVersionAvailable: 'Đã có version mới',
    contactAddress: 'Địa chỉ liên hệ',
    fromToDate: 'Từ {fromDate} đến {toDate}',
    pleaseEnterApprovalQuantity: 'Vui lòng nhập số lượng duyệt',
    pleaseEnterFullInfo: 'Vui lòng nhập đủ thông tin\nđể tiếp tục!',
    pleaseEnterFullNameBeforeSign: 'Vui lòng nhập họ tên trước khi ký số',
    pleaseEnterIdCardBeforeSign: 'Vui lòng nhập CCCD/Căn cước trước khi ký số',
    pleaseEnterFeedbackContent: 'Vui lòng nhập nội dung góp ý/ phản hồi',
    pleaseUpdatePersonalInfoBeforeFeedback:
        'Vui lòng cập nhật thông tin cá nhân trước khi gửi phản hồi!',
    pleaseUpdatePersonalInfoBeforeRegister:
        'Vui lòng cập nhật thông tin cá nhân trước khi đăng ký hiến máu!',
    pleaseUpdatePersonalInfoBeforeBuyBlood:
        'Vui lòng cập nhật thông tin cá nhân trước khi tạo yêu cầu nhượng máu!',
    saveChanges: 'Lưu thay đổi',
    selectDateTime: 'Chọn ngày giờ',
    selectBloodDonationSchedule: 'Chọn lịch hiến máu',
    notEnterOldPassword: 'Chưa nhập mật khẩu cũ',
    notEnterPassword: 'Chưa nhập mật khẩu',
    changePasswordSuccess: 'Thay đổi mật khẩu thành công',
    changePasswordFailed: 'Thay đổi mật khẩu thất bại',
    numberOfBloodDonations: 'Số lần hiến máu',
    welcomeBack: 'Chào mừng bạn quay trở lại!',
    notDonatedYet: 'Bạn chưa hiến máu lần nào!',
    daysSinceLastDonation: 'Đã {days} ngày rồi bạn chưa hiến máu!\n',
    lastDonation: 'Lần hiến máu gần nhất của bạn: {date}',
    notificationTitle: 'Thông báo',
    cannotFindRoute: 'Không tìm thấy đường đi',
    pleaseSelectBloodUnit: 'Vui lòng chọn đơn vị cấp máu.',
    approveBuyBloodSuccess: 'Duyệt yêu cầu nhượng máu thành công.',
    rejectBuyBloodSuccess: 'Từ chối yêu cầu nhượng máu thành công.',
    sendSuccess: 'Gửi thành công!',
    notEnterFullName: 'Chưa nhập họ tên',
    invalidFullName: 'Họ tên không hợp lệ',
    notEnterUsername: 'Chưa nhập tên tài khoản',
    notEnterPhone: 'Chưa nhập số điện thoại',
    invalidPhone: 'Số điện thoại không đúng',
    qrCodeReadSuccess: 'Đã lấy thông tin từ QR code thành công!',
    qrCodeReadError: 'Lỗi khi đọc thông tin từ QR code!',
    qrScanError: 'Lỗi khi quét QR code!',
    signatureSavedSuccess: 'Đã lưu chữ ký thành công!',
    signatureNotComplete: 'Chưa hoàn thành chữ ký',
    signatureError: 'Lỗi khi thực hiện chữ ký số:',
    updateAccountSuccess: 'Cập nhật tài khoản thành công',
    updateAccountFailed: 'Cập nhật tài khoản thất bại',
    invalidIdCard: 'CCCD/Căn cước không đúng định dạng!',
    cancelRegisterSuccess: 'Hủy đăng ký thành công',
    cancelRegisterFailed: 'Hủy đăng ký thất bại',
    cancelBuyBloodSuccess: 'Hủy yêu cầu nhượng máu thành công',
    cancelBuyBloodFailed: 'Hủy yêu cầu nhượng máu thất bại',
    updateInventorySuccess: 'Cập nhật thông tin tồn kho thành công.',
    errorOccurredPleaseRetry: 'Có lỗi xãy ra, xin vui long thử lại sau',
    sendOtpSuccess: 'Gửi OTP thành công',
    otpIncorrect: 'Mã OTP không chính xác',
    otpSendSuccess: 'Gửi OTP thành công',
    time: 'Lần',
    invalidUsername:
        'Tên đăng nhập phải là CCCD/Căn cước có độ dài 9 hoặc 12 ký tự!',
    usernameWithIdCard: 'CCCD/Căn cước',
    scanIdCardToGetInfo: 'Quét CCCD/Căn cước để lấy thông tin',
    publishedDate: 'Ngày đăng',
    fieldRequired: 'Trường này là bắt buộc',
    qrCodeRegistration: 'Mã QR đăng ký hiến máu',
    eventId: 'Id đợt',
    registrationId: 'Id đăng ký',
    registeringToDonate: 'Bạn đang đăng ký hiến',
    youAreRegisteringToDonate: 'Bạn đang đăng ký hiến {type}',
    registeredCount: 'Đã đăng ký: {registered}/{total}',
    platelets: 'tiểu cầu',
    blood: 'máu',
    // Question Answer - Titles
    qaDonationLocation: 'ĐỊA ĐIỂM HIẾN MÁU Ở ĐÂU?',
    qaDonationTime: 'THỜI GIAN TIẾP NHẬN ĐĂNG KÝ HIẾN MÁU NHƯ THẾ NÀO?',
    qaDonationConditions: 'TIÊU CHUẨN KHI HIẾN MÁU, TIỂU CẦU?',
    qaDonationNotes: 'LƯU Ý GÌ KHI ĐẾN HIẾN MÁU?',
    qaDonationHandbook: 'CẨM NANG',
    qaDonationBenefits: 'QUYỀN LỢI KHI HIẾN MÁU TÌNH NGUYỆN?',
    qaBloodTesting: 'MÁU CỦA TÔI SẼ LÀM NHỮNG XÉT NGHIỆM GÌ?',
    qaWhyNeedIdCard: 'TẠI SAO HIẾN MÁU PHẢI CÓ CCCD/CĂN CƯỚC?',
    qaCanGetInfected: 'KHI HIẾN MÁU CÓ THỂ BỊ NHIỄM BỆNH KHÔNG?',
    qaFeelUnwell: 'CẢM THẤY KHÔNG KHỎE SAU KHI HIẾN MÁU?',
    qaSwellingSigns: 'CÓ DẤU HIỆU SƯNG, PHÙ NƠI VẾT CHÍCH?',
    qaPrepareForDonation:
        'NGÀY MAI TÔI SẼ HIẾN MÁU, TÔI NÊN CHUẨN BỊ NHƯ THẾ NÀO?',
    qaAbnormalSituation:
        'KHI PHÁT HIỆN BẤT THƯỜNG, CẢM THẤY KHÔNG AN TOÀN VỚI TÚI MÁU VỪA HIẾN?',
    qaWhyNeedBlood: 'TẠI SAO CÓ NHIỀU NGƯỜI CẦN PHẢI ĐƯỢC TRUYỀN MÁU?',
    qaIsDonationHarmful: 'HIẾN MÁU NHÂN ĐẠO CÓ HẠI ĐẾN SỨC KHOẺ KHÔNG?',
    qaPostponeDonation: 'NHỮNG TRƯỜNG HỢP NÀO CẦN PHẢI TRÌ HOÃN HIẾN MÁU?',
    // Question Answer - Content
    qaBloodTransfusionCenter: 'TRUNG TÂM TRUYỀN MÁU CHỢ RẪY',
    qaMorning: 'Buổi sáng',
    qaAfternoon: 'Buổi chiều',
    qaAge: 'Tuổi',
    qaWeight: 'Cân nặng',
    qaDistanceBetweenDonations: 'Khoảng cách giữa 2 lần hiến máu',
    qaDistanceBetweenPlatelets: 'Khoảng cách giữa 2 lần hiến tiểu cầu',
    qaDiseases: 'Bệnh lý',
    qaMenstrualCycle: 'Kết thúc chu kỳ kinh nguyệt',
    qaForFemale: '(đối với nữ)',
    qaEatNormally: 'Ăn uống bình thường và',
    qaBringIdCard: 'Khi đến hiến máu, quý vị vui lòng mang theo CCCD/Căn cước.',
    qaParkingLocation:
        'Người hiến máu gửi xe tại cổng 3 – Bệnh viện Chợ Rẫy, đường Thuận Kiều.',
    qaBenefitsContent:
        'Quyền lợi và chế độ đối với người hiến máu tình nguyện theo Thông tư số 05/2017/TT-BYT Quy định giá tối đa và chi phí phục vụ cho việc xác định giá một đơn vị máu toàn phần, chế phẩm máu đạt tiêu chuẩn:\n• Được khám và tư vấn sức khỏe miễn phí.\n• Được kiểm tra và thông báo kết quả các xét nghiệm máu (hoàn toàn bí mật): nhóm máu, HIV, virut viêm gan B, virut viêm gan C, giang mai, sốt rét. Trong trường hợp người hiến máu có nhiễm hoặc nghi ngờ các mầm bệnh này thì sẽ được Bác sỹ mời đến để tư vấn sức khỏe.\n• Được bồi dưỡng và chăm sóc theo các quy định hiện hành:\n   + Phục vụ ăn nhẹ tại chỗ: tương đương 30.000 đồng.\n   + Hỗ trợ chi phí đi lại (bằng tiền mặt): 50.000 đồng.\n   + Lựa chọn nhận quà tặng bằng hiện vật có giá trị như sau: Một đơn vị máu thể tích 250 ml: 100.000 đồng. Một đơn vị máu thể tích 350 ml: 150.000 đồng. Một đơn vị máu thể tích 450 ml: 180.000 đồng.\n   + Được cấp giấy chứng nhận hiến máu tình nguyện của Ban chỉ đạo hiến máu nhân đạo Tỉnh, Thành phố. Ngoài giá trị về mặt tôn vinh, giấy chứng nhận hiến máu có giá trị bồi hoàn máu, số lượng máu được bồi hoàn lại tối đa bằng lượng máu người hiến máu đã hiến. Giấy Chứng nhận này có giá trị tại các bệnh viện, các cơ sở y tế công lập trên toàn quốc.',
    qaTestingContent:
        'Tất cả những đơn vị máu thu được sẽ được kiểm tra nhóm máu (hệ ABO, hệ Rh), HIV, virus viêm gan B, virus viêm gan C, giang mai, sốt rét.',
    qaTestingResult:
        'Bạn sẽ được thông báo kết quả, được giữ kín và được tư vấn (miễn phí) khi phát hiện ra các bệnh nhiễm trùng nói trên.',
    qaIdCardReason:
        'Mỗi đơn vị máu đều phải có hồ sơ, trong đó có các thông tin về người hiến máu.\nTheo quy định, đây là một thủ tục cần thiết trong quy trình hiến máu để đảm bảo tính xác thực thông tin về người hiến máu.',
    qaCannotGetInfected:
        'Kim dây lấy máu vô trùng, chỉ sử dụng một lần cho một người, vì vậy không thể lây bệnh cho người hiến máu.',
    qaUnwellSymptoms:
        'Sau khi hiến máu, nếu có các triệu chứng chóng mặt, mệt mỏi, buồn nôn,... hãy liên hệ ngay cho đơn vị tiếp nhận máu để được hỗ trợ về mặt y khoa.',
    qaSwellingInstructions:
        'Sau khi hiến máu, nếu bạn có các dấu hiệu sưng, phù nơi vết chích. Xin đừng quá lo lắng, hãy chườm lạnh ngay vị trí sưng đó và theo dõi các dấu hiệu trên, nếu không giảm sau 24 giờ hãy liên hệ lại cho đơn vị tiếp nhận máu để được hỗ trợ.',
    qaPrepareInstructions:
        '• Tối nay bạn không nên thức quá khuya (ngủ trước 23:00).\n• Nên ăn và không uống rượu, bia trước khi hiến máu.\n• Mang giấy CCCD/Căn cước, đủ giấy tờ tùy thân và thẻ hiến máu (nếu có) khi đi hiến máu.',
    qaAbnormalInstructions:
        'Sau khi tham gia hiến máu, nếu phát hiện có bất cứ điều gì khiến bạn cảm thấy không an toàn với túi máu vừa hiến (chợt nhớ ra 1 hành vi nguy cơ, có sử dụng loại thuốc nào đó mà bạn quên báo bác sĩ khi thăm khám, có xét nghiệm \'DƯƠNG TÍNH\' với SarS-CoV-2 bằng kỹ thuật test nhanh hoặc Real time RT-PCR,...) vui lòng báo lại cho đơn vị tiếp nhận túi máu nơi mà bạn đã tham gia hiến.',
    qaWhyNeedBloodReason:
        'Mỗi giờ có hàng trăm người bệnh cần phải được truyền máu vì:\n• Bị mất máu do chấn thương, tai nạn, thảm hoạ, xuất huyết tiêu hoá...\n• Do bị các bệnh gây thiếu máu, chảy máu: ung thư máu, suy tuỷ xương, máu khó đông...\n• Các phương pháp điều trị hiện đại cần truyền nhiều máu: phẫu thuật tim mạch, ghép tạng...',
    qaHealthContent:
        'Hiến máu theo hướng dẫn của thầy thuốc không có hại cho sức khỏe. Điều đó đã được chứng minh bằng các cơ sở khoa học và cơ sở thực tế:\n• Máu có nhiều thành phần, mỗi thành phần chỉ có đời sống nhất định và luôn luôn được đổi mới hằng ngày. Ví dụ: Hồng cầu sống được 120 ngày, huyết tương thường xuyên được thay thế và đổi mới. Cơ sở khoa học cho thấy, nếu mỗi lần hiến dưới 1/10 lượng máu trong cơ thể thì không có hại đến sức khỏe.\n• Nhiều công trình nghiên cứu đã chứng minh rằng, sau khi hiến máu, các chỉ số máu có thay đổi chút ít nhưng vẫn nằm trong giới hạn sinh lý bình thường không hề gây ảnh hưởng đến các hoạt động thường ngày của cơ thể.\n• Thực tế đã có hàng triệu người hiến máu nhiều lần mà sức khỏe vẫn hoàn toàn tốt. Trên thế giới có người hiến máu trên 400 lần. Ở Việt Nam, người hiến máu nhiều lần nhất đã hiến gần 100 lần, sức khỏe hoàn toàn tốt.\n• Như vậy, mỗi người nếu thấy sức khoẻ tốt, không có các bệnh lây nhiễm qua đường truyền máu, đạt tiêu chuẩn hiến máu thì có thể hiến máu từ 3-4 lần trong một năm, vừa không ảnh hưởng xấu đến sức khoẻ của bản thân, vừa đảm bảo máu có chất lượng tốt, an toàn cho người bệnh.',
    qaPostponeContent:
        '• Những người phải trì hoãn hiến máu trong 12 tháng kể từ thời điểm:\n   + Phục hồi hoàn toàn sau các can thiệp ngoại khoa.\n   + Khỏi bệnh sau khi mắc một trong các bệnh sốt rét, giang mai, lao, uốn ván, viêm não, viêm màng não.\n   + Kết thúc đợt tiêm vắc xin phòng bệnh dại sau khi bị động vật cắn hoặc tiêm, truyền máu, chế phẩm máu và các chế phẩm sinh học nguồn gốc từ máu.\n   + Sinh con hoặc chấm dứt thai nghén.\n\n• Những người phải trì hoãn hiến máu trong 06 tháng kể từ thời điểm:\n   + Xăm trổ trên da.\n   + Bấm dái tai, bấm mũi, bấm rốn hoặc các vị trí khác của cơ thể.\n   + Phơi nhiễm với máu và dịch cơ thể từ người có nguy cơ hoặc đã nhiễm các bệnh lây truyền qua đường máu.\n   + Khỏi bệnh sau khi mắc một trong các bệnh thương hàn, nhiễm trùng huyết, bị rắn cắn, viêm tắc động mạch, viêm tắc tĩnh mạch, viêm tuỷ xương, viêm tụy.\n\n• Những người phải trì hoãn hiến máu trong 04 tuần kể từ thời điểm:\n   + Khỏi bệnh sau khi mắc một trong các bệnh viêm dạ dày ruột, viêm đường tiết niệu, viêm da nhiễm trùng, viêm phế quản, viêm phổi, sởi, ho gà, quai bị, sốt xuất huyết, kiết lỵ, rubella, tả, quai bị.\n   + Kết thúc đợt tiêm vắc xin phòng rubella, sởi, thương hàn, tả, quai bị, thủy đậu, BCG.\n\n• Những người phải trì hoãn hiến máu trong 07 ngày kể từ thời điểm:\n   + Khỏi bệnh sau khi mắc một trong các bệnh cúm, cảm lạnh, dị ứng mũi họng, viêm họng, đau nửa đầu Migraine.\n   + Tiêm các loại vắc xin, trừ các loại đã được quy định tại Điểm c Khoản 1 và Điểm b Khoản 3 Điều này.\n\n• Một số quy định liên quan đến nghề nghiệp và hoạt động đặc thù của người hiến máu: những người làm một số công việc và thực hiện các hoạt động đặc thù sau đây chỉ hiến máu trong ngày nghỉ hoặc chỉ được thực hiện các công việc, hoạt động này sau khi hiến máu tối thiểu 12 giờ:\n   + Người làm việc trên cao hoặc dưới độ sâu: phi công, lái cần cẩu, công nhân làm việc trên cao, người leo núi, thợ mỏ, thủy thủ, thợ lặn.\n   + Người vận hành các phương tiện giao thông công cộng: lái xe buýt, lái tàu hoả, lái tàu thuỷ.\n   + Các trường hợp khác: vận động viên chuyên nghiệp, người vận động nặng, tập luyện nặng.',
    qaDonateBlood: 'Hiến máu',
    qaDonatePlatelets: 'Hiến tiểu cầu',
    qaWeekendDonation: 'Hiến máu cuối tuần',
    qaFromMondayToFriday: 'từ thứ 2 đến thứ 6 hàng tuần',
    qaSecondSunday: 'Chủ nhật tuần thứ 2 hàng tháng',
    qaMayStopEarly: '(Có thể ngừng nhận đăng ký sớm hơn khi nhận nhu cầu)',
    at: 'tại:',
    timeFrom: 'Thời gian từ',
    timeFromDate: 'Thời gian từ {time} ngày {date}',
    day: 'ngày',
    noDataFromToDate: 'Không có dữ liệu\r\ntừ {fromDate} đến {toDate}',
    updateIdCard: 'Cập nhật CCCD/Căn cước',
    here: 'Tại đây',
    toViewDonationRegisterHistory: ' để xem lịch sử đăng ký hiến máu của bạn.',
    toViewDonationHistory: ' để xem lịch sử hiến máu của bạn.',
    apply: 'Áp dụng',
    selectTimeRange: 'Chọn khoảng thời gian',
    contactInformation: 'THÔNG TIN LIÊN HỆ',
    contactViaFanpage: 'LIÊN HỆ QUA FANPAGE',
    sendRequestDirectly: 'GỬI YÊU CẦU TRỰC TIẾP',
    forgotPassword: 'Bạn quên mật khẩu?',
    pleaseFillInfoBelow:
        'Vui lòng điền các thông tin bên dưới.\nTrung tâm sẽ liên hệ để cấp lại mật khẩu cho bạn!',
    send: 'Gửi',
    address: 'Địa chỉ: ',
    phone: 'Điện thoại: ',
    extension: ' - số nội bộ ',
    or: ' hoặc ',
    contactDuringBusinessHours: ' (liên hệ trong giờ hành chính).',
    version: 'Phiên bản: ',
    updateDate: 'Ngày cập nhật: ',
    bloodTransfusionCenter: 'TRUNG TÂM TRUYỀN MÁU CHỢ RẪY',
    bloodTransfusionCenterAddress:
        'Bệnh viện Chợ Rẫy - 201B Nguyễn Chí Thanh, phường 12, Quận 5, TP.Hồ Chí Minh.',
    thankYouForUsingApp:
        'Cảm ơn bạn đã sử dụng ứng dụng của chúng tôi!\nHãy để lại đánh giá để cải thiện thêm',
    rateApp: 'ĐÁNH GIÁ APP',
    // Biometric
    loginWithBiometric: 'Đăng nhập bằng vân tay, Face ID',
    biometricNotAvailable: 'Thiết bị không hỗ trợ xác thực sinh trắc học',
    biometricNotEnrolled:
        'Chưa có thông tin đăng nhập. Vui lòng đăng nhập trước.',
    biometricAuthFailed: 'Xác thực sinh trắc học thất bại',
    biometricAuthSuccess: 'Đăng nhập thành công',
    enableBiometricLogin: 'Bật đăng nhập bằng sinh trắc học',
    biometricAuth: 'Xác thực sinh trắc học',
    biometricAuthReason: 'Vui lòng xác thực để đăng nhập',
    biometricDeviceNotSupported:
        'Thiết bị của bạn không hỗ trợ vân tay/Face ID. Vui lòng đăng nhập bằng tên đăng nhập và mật khẩu.',
    biometricNotLoggedIn:
        'Bạn chưa đăng nhập. Vui lòng đăng nhập bằng tên đăng nhập và mật khẩu trước khi sử dụng đăng nhập bằng vân tay/Face ID.',
    biometricSessionExpired:
        'Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại bằng tên đăng nhập và mật khẩu.',
    biometricLoginWithAccount: 'Đăng nhập với tài khoản: {account}\n\n{reason}',
    biometricAuthNotFound:
        'Không tìm thấy thông tin đăng nhập. Vui lòng đăng nhập lại.',
    biometricTokenInvalid: 'Token không hợp lệ. Vui lòng đăng nhập lại.',
    biometricError: 'Lỗi: {error}',
    biometricLockedToOtherAccount:
        'Đăng nhập vân tay đã gắn với tài khoản khác. Vui lòng xóa đăng nhập vân tay trước khi đổi tài khoản.',
    loginUsernameRequired: 'Chưa nhập tên tài khoản hoặc CCCD/Căn cước',
    loginPasswordRequired: 'Chưa nhập mật khẩu',
    registerFullNameRequired: 'Chưa nhập họ tên',
    registerFullNameInvalid: 'Họ tên không hợp lệ',
    registerUsernameRequired: 'Chưa nhập tên tài khoản',
    registerPasswordRequired: 'Chưa nhập mật khẩu',
    registerPasswordMinLength: 'Mật khẩu phải từ 6 ký tự',
    registerPasswordNotMatch: 'Mật khẩu và xác nhận mật khẩu không giống nhau',
    registerFullNameAccountRequired: 'Chưa nhập họ tên tài khoản',
    registerPhoneRequired: 'Chưa nhập số điện thoại',
    registerPhoneInvalid: 'Số điện thoại không đúng',
    registerSuccess: 'Đăng ký tài khoản thành công!',
    registerFailed: 'Đăng ký tài khoản thất bại!',
    alreadyRegisteredSameDay:
        'Bạn đã đăng ký lịch hiến máu (khác) trong ngày. Vui lòng kiểm tra và hủy trước khi đăng ký mới!',
    notEnoughDaysToDonate:
        'Bạn chưa đủ số ngày quy định ({days} ngày) để hiến {type}.\nLần hiến {type} gần nhất của bạn là {date}',
    registerDonateBloodSuccess: 'Đăng ký thành công',
    scanQRSuccess: 'Quét mã QR thành công!',
    scanQRError: 'Lỗi khi đọc mã QR',
    changePasswordOldPasswordRequired: 'Chưa nhập mật khẩu cũ',
    forgotPasswordInfoSent: 'Thông tin đã được gửi!',
    forgotPasswordSendError: 'Lỗi gửi thông tin',
    homeDaysSinceLastDonation: 'Đã {days} ngày rồi bạn chưa hiến máu!\n',
    homeLastDonationDate: 'Lần hiến máu gần nhất của bạn: {date}',
    homeWelcomeBack: 'Chào mừng bạn quay trở lại!',
    homeNoDonationYet: 'Bạn chưa hiến máu lần nào!',
    profileIDCardInvalidFormat: 'CCCD/Căn cước không đúng định dạng!',
    profilePhoneInvalidFormat: 'Số điện thoại không đúng định dạng!',
    profileDataUpdatedFromIDCard:
        'Dữ liệu đã được cập nhật theo\r\nCCCD/Căn cước.\r\nNếu bạn muốn thay đổi vui lòng liên hệ\r\nTrung Tâm Truyền Máu Chợ Rẫy',
    profileBuyBloodAccountNote:
        'Đây là tài khoản đăng ký nhượng máu\r\nKhông thể chỉnh sửa thông tin.',
    feedbackSendSuccess: 'Gửi thành công!',
    feedbackPleaseUpdateInfoBeforeSend:
        'Vui lòng cập nhật thông tin cá nhân trước khi gửi phản hồi!',
    registerBuyBloodNoInventoryToday:
        'Hiện không có tồn trong ngày {date}, Vui lòng chọn ngày khác!',
    registerBuyBloodQuantityExceeded:
        'Số lượng {bloodType} của {productType} ({quantity}) không được lớn hơn ({available})',
    registerBuyBloodPleaseSelectUnit: 'Vui lòng chọn đơn vị cấp máu.',
    routeNotFound: 'Không tìm thấy đường đi',
    donationScheduleTestResultReactive:
        'Kết quả xét nghiệm gần nhất của bạn là Phản ứng, nên không thể đăng ký hiến máu',
    donationScheduleConfirmDonate: 'Xác nhận hiến {type}',
    donationScheduleConfirmDonateMessage:
        'Đồng ý đăng ký hiến {type}\rtại địa điểm này?',
    managementNoInventoryToday:
        'Hiện không có tồn trong ngày {date}, vui lòng cập nhật tồn!',
    managementErrorGettingInventory:
        'Lỗi lấy tồn trong ngày {date}, vui lòng liên hệ Kỹ thuật viên!',
    managementNotification: 'Thông báo',
    managementPleaseEnterApproveQuantity: 'Vui lòng nhập số lượng duyệt',
    managementApproveBuyBloodSuccess: 'Duyệt yêu cầu nhượng máu thành công.',
    managementRejectBuyBloodSuccess: 'Từ chối yêu cầu nhượng máu thành công.',
    managementConfirmRejectRequest: 'Xác nhận từ chối phiếu này',
    historyCancelBuyBloodSuccess: 'Hủy yêu cầu nhượng máu thành công',
    historyCancelBuyBloodFailed: 'Hủy yêu cầu nhượng máu thất bại',
    historyConfirmCancelBuyBlood: 'Xác nhận hủy yêu cầu nhượng máu này?',
    exportQRCode: 'Xuất mã QR',
    cancelRegistration: 'Hủy đăng ký',
    status: 'Tình trạng',
    noDataFromDateToDate:
        'Không có dữ liệu từ ngày {fromDate} đến ngày {toDate}',
    cancelRegistrationSuccess: 'Hủy đăng ký thành công',
    receivedAt: 'Tiếp nhận lúc:',
    processedAt: 'Điều chế lúc:',
    timeWithIndex: 'Lần {index}',
    selectImageFromGallery: 'Chọn ảnh từ thư viện',
    noQRCodeFoundInImage: 'Không tìm thấy QR code trong ảnh đã chọn',
    failedToReadQRFromImage: 'Không thể đọc QR code từ ảnh',
    // Digital signature flow
    receptionStep: 'Tiếp nhận',
    receptionStepTitle: 'Tiếp nhận - Người hiến máu ký tên',
    receptionStepDescription: 'Vui lòng ký tên của bạn để xác nhận tiếp nhận',
    chooseSigningMethod: 'Chọn phương thức ký:',
    handSignature: 'Chữ ký tay',
    digitalSignatureSmartCA: 'Chữ ký số SmartCA',
    pleaseSignYourNameBelow: 'Vui lòng ký tên của bạn vào ô bên dưới',
    signWithSmartCA: 'Ký số bằng SmartCA',
    pleaseRegisterBeforeSigning: 'Vui lòng đăng ký trước khi ký số.',
    digitalSignatureDescription:
        'Chữ ký số sẽ được thực hiện qua hệ thống SmartCA. Bạn sẽ được yêu cầu xác thực để hoàn tất quá trình ký số.',
    digitalSignatureFailed: 'Ký số thất bại',
    goToSignatureScreen: 'Vào màn ký tên',
    donorSignature: 'Chữ ký người hiến máu',
    measureVitalSigns: 'Đo chỉ số sinh tồn',
    measureVitalSignsTitle: 'Đo chỉ số sinh tồn',
    bloodPressure: 'Huyết áp',
    heartRate: 'Nhịp tim',
    temperature: 'Nhiệt độ',
    systolic: 'Tâm thu',
    diastolic: 'Tâm trương',
    bpm: 'lần/phút',
    celsius: '°C',
    preDonationTest: 'Xét nghiệm trước hiến máu',
    preDonationTestTitle: 'Xét nghiệm trước hiến máu - Nhân viên ký số',
    staffSignature: 'Chữ ký nhân viên',
    doctorConfirmation: 'Bác sĩ xác nhận',
    doctorConfirmationTitle: 'Bác sĩ xác nhận - Bác sĩ ký số',
    doctorSignature: 'Chữ ký bác sĩ',
    nurseBloodDraw: 'Điều dưỡng rút máu',
    nurseBloodDrawTitle: 'Điều dưỡng rút máu - Điều dưỡng ký số',
    nurseSignature: 'Chữ ký điều dưỡng',
    completeBloodDonation: 'Hoàn thành hiến máu',
    thankYouLetterSent: 'Đã gửi thư cảm ơn',
    pleaseEnterVitalSigns: 'Vui lòng nhập đầy đủ chỉ số sinh tồn',
    pleaseSignToContinue: 'Vui lòng ký để tiếp tục',
    stepCompleted: 'Hoàn thành bước thành công',
    stepFailed: 'Bước thất bại. Vui lòng thử lại',
    updateStatusSuccess: 'Cập nhật trạng thái thành công',
    updateStatusFailed: 'Cập nhật trạng thái thất bại',
    sendThankYouLetterSuccess: 'Gửi thư cảm ơn thành công',
    sendThankYouLetterFailed: 'Gửi thư cảm ơn thất bại',
    bloodDonationCompleted: 'Hoàn thành hiến máu thành công!',
    updateSignature: 'Cập nhật chữ ký',
    clear: 'Xóa',
    pleaseSignAsStaff: 'Vui lòng ký tên với tư cách nhân viên',
    pleaseSignAsDoctor: 'Vui lòng ký tên với tư cách bác sĩ',
    pleaseSignAsNurse: 'Vui lòng ký tên với tư cách điều dưỡng',
    feedbackTitle: 'Góp ý/ phản hồi',
    feedbackIntroMessage:
        'Chúng tôi đánh giá cao phản hồi của bạn. Vui lòng điền vào mẫu dưới đây.',
    feedbackEmailInvalid: 'Vui lòng nhập địa chỉ email hợp lệ',
    feedbackContentPlaceholder: 'Nội dung góp ý/ phản hồi',
  };

  void init(
      {required List<MapLocale> mapLocales,
      AppLanguage initLanguage = AppLanguage.en}) {
    localization.onTranslatedLanguage = onTranslatedLanguage;
    localization.init(
        mapLocales: mapLocales, initLanguageCode: initLanguage.languageCode);
  }

  void onTranslatedLanguage(Locale? locale) {}
}
