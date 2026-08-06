# Course Pilot

Course Pilot is an AI-powered desktop application that helps students organize information from university course syllabi. By uploading a syllabus PDF, the application automatically extracts important course information, exam dates, grading policies, instructor details, and other key information into a clean and organized interface.

## Features

- Upload and analyze course syllabus PDFs
- AI-powered syllabus analysis using the OpenAI API
- Automatically extracts:
  - Course information
  - Instructor information
  - Office hours
  - Exam dates
  - Grade weight breakdowns
- Built-in AI assistant for answering syllabus-related questions
- Web search fallback when requested information is not available in the syllabus
- Generates helpful study resources
- Saves course information locally for future use
- Modern WPF desktop interface

## Technologies

- C#
- WPF (.NET)
- OpenAI API
- PdfPig
- JSON Serialization
- Visual Studio 2022

## Installation

1. Download the latest CoursePilotSetup.exe from the Releases page.

2. Run the installer.

3. Launch CoursePilot from the Start Menu or Desktop shortcut.

4. Upload a course syllabus PDF and start organizing your semester.

## Requirements

- Windows 10/11 (64-bit)

- Internet connection

## Security

CoursePilot follows a client-server architecture.

The desktop application communicates with a hosted ASP.NET Core API, which securely handles all AI requests. Users never need to configure or expose their own OpenAI API key.

No API keys or secrets are included in this repository.

## Planned Features

Future development may include:

- Canvas LMS integration
- Assignment calendar
- GPA planner
- Multi-course dashboard
- Cloud synchronization
- Mobile companion application

## License

This project is provided for educational and portfolio purposes.

## Author

CJ Thomas

Computer Science Student

GitHub: https://github.com/CJThomasDev

Portfolio: https://cjthomasdev.github.io/
