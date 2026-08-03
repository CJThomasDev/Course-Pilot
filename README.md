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

1. Clone the repository.

```bash
git clone https://github.com/CJThomasDev/Course-Pilot.git
```

2. Open the solution in Visual Studio.

3. Restore the required NuGet packages.

4. Create an OpenAI API key.

5. Create an environment variable named:

```
OPENAI_API_KEY
```

6. Set the value of the variable to your OpenAI API key.

7. Build and run the project.

## Requirements

- Windows
- .NET
- Visual Studio 2022 or newer
- OpenAI API Key

## Security

For security reasons, no API keys are included in this repository.

To use the AI features, users must create their own OpenAI API key and store it as an environment variable named:

```
OPENAI_API_KEY
```

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
