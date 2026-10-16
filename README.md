#  RaceDay

A RESTful Web API for managing running, walking, and cycling events — built with **ASP.NET Core**, **Entity Framework Core**, and **SQL Server**. Organisers create events and categories; participants enroll and receive results.


---

##  Overview

**RaceDay** is a backend API that lets:

- **Organisers** create events (races, walks, rides), add categories (e.g. Senior, Junior), and capture participants' finish times and positions.
- **Participants** browse events, enroll in categories, and view their own race results.

The project demonstrates a clean layered architecture with a strong emphasis on **testability** — every layer can be tested in isolation using an in-memory database.

---

## Features

### Authentication
- User registration with role selection (Organiser or Participant)
- Login with session-based authentication
- Passwords hashed with **BCrypt** (never stored in plain text)
- Logout clears session

### Events
- Public listing of all events (authenticated users)
- Full CRUD for Organisers (Create, Read, Update, Delete)
- Ownership validation — only the Organiser who created an event can edit/delete it

### Categories
- Organisers define categories per event (e.g. "Senior", "Junior", "Open")
- Public read access for authenticated users

### Enrollments & Results
- Participants enroll into events with a specific category
- Organisers record finish times and positions
- Participants view their own results

### Testing
- Full xUnit test suite using the **EF Core InMemory** provider
- Tests cover password hashing, role assignment, enrollment relationships, and event persistence

<img width="1366" height="728" alt="image" src="https://github.com/user-attachments/assets/4d1949f6-9af5-43b2-9270-0fbce1ba61f5" />

<img width="1366" height="728" alt="image" src="https://github.com/user-attachments/assets/c4dae7ea-7ca7-4324-baae-bc959722b55e" />



