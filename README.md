# amqpTranslator

[![License](https://img.shields.io/badge/license-MIT-blue)](https://opensource.org/licenses/MIT)  
[![.NET](https://img.shields.io/badge/.NET-6.0-purple?logo=dotnet)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)  
[![C#](https://img.shields.io/badge/language-C%23-239120?logo=csharp&logoColor=white)]()  
[![Build](https://img.shields.io/badge/build-passing-brightgreen)]()  

A C#/.NET utility that translates AMQP (Advanced Message Queuing Protocol) messages into human-readable text.
This project was built as a lightweight tool for inspecting and understanding AMQP frames without needing to run a broker.


## ✨ Features

- **AMQP Translation** – Converts raw AMQP message data into a readable format.
- **C# Utility + MVC UI** – Core logic in Translator.cs, exposed through a simple ASP.NET Core MVC interface.
- **Offline Friendly** – Does not connect to or publish back to RabbitMQ; safe for standalone decoding/inspection.


## 📋 Requirements

- .NET 6 SDK or later
- Any OS supported by .NET (Windows, macOS, Linux)


## 🚀 Getting Started

Clone the repo:
```bash
git clone https://github.com/quinise/amqpTranslator.git
cd amqpTranslator
```

Run the project:
```bash
dotnet run
```

This will start the ASP.NET Core app and make the translator UI available locally.

---

## 📦 Example Usage
Paste or load an AMQP message string into the UI, and the app will display a parsed, readable breakdown of the message fields.

---


## 🛠️ Tech Stack
- **C# / .NET 6**  
- **ASP.NET Core MVC**  
- **Custom Translation Logic** (`Translator.cs`)  


## 🤝 Contributing

Pull requests and issues are welcome! If you find an AMQP message type that isn’t decoded correctly, feel free to open a ticket or suggest an improvement.

## 📄 License

Distributed under the [MIT License](https://opensource.org/licenses/MIT).
