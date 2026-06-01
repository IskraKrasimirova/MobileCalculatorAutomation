# MobileCalculatorAutomation

**MobileCalculatorAutomation** is a mobile UI automation project built with **C#**, **.NET 8**, and **Appium**, designed to automate an Android Calculator application.  
The project follows the **Page Object Model (POM)** pattern and is structured for future expansion to iOS.

---

## 🚀 Technologies Used

- C# / .NET 8
- Appium
- NUnit
- Page Object Model (POM)
- Android UI Automation
- Visual Studio 2022

---

## 📁 Project Structure

```
MobileAutomation/
│
├── MobileUITests/
│   ├── Drivers/          # Appium driver factory and driver setup
│   ├── Models/           # AppiumSettings and configuration models
│   ├── Pages/            # Page Object classes (BasePage, CalculatorPage)
│   ├── Tests/            # NUnit test classes for calculator operations
│   ├── Utils/            # ConfigReader and helper utilities
│   ├── appsettings.json  # Appium capabilities and configuration
│   └── MobileUITests.csproj
│
└── MobileAutomation.sln
```

---

## 🧪 Test Coverage

The project includes automated tests for core calculator operations:

- Addition  
- Subtraction  
- Multiplication  
- Division  
- Mixed Operations

Each test uses shared setup logic from `BaseTest` and interacts with the UI through POM classes.

---

## ⚙️ Configuration

Appium configuration is stored in:

```
MobileUITests/appsettings.json
```

This file contains:

- deviceName  
- platformVersion  
- appPackage / appActivity  
- automationName  
- Appium server URL  

---

## ▶️ Running the Tests

1. Install **.NET 8 SDK**  
2. Start the **Appium server**  
3. Connect an Android device or emulator  
4. Run:

```
dotnet test
```

---

## 📌 Planned Improvements

- Add iOS automation  
- Create unified driver layer for Android + iOS  
- Integrate Allure or ExtentReports  
- Add GitHub Actions CI pipeline  

---

## 📄 License

MIT License
