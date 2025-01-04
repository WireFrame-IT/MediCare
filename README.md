# MediCare Installation Guide

Welcome to the MediCare project! This guide will help you set up and run the application. Follow the steps below to ensure a smooth installation and deployment process.

---

## About MediCare

MediCare is a comprehensive healthcare management system designed to simplify and enhance the delivery of medical services. It provides robust tools for scheduling appointments, managing patient records, issuing prescriptions, and generating reports. MediCare is built with scalability, security, and efficiency in mind, making it an ideal solution for clinics, hospitals, and healthcare providers.

---

## Prerequisites

Before you begin, make sure you have the following installed on your system:

- **.NET SDK 8.0 or later**
  Ensure that you have the latest version of the .NET SDK installed to build and run the application.

- **SQL Server**
  A running instance of SQL Server is required to manage the application's database.

- **IIS (Internet Information Services)**
  IIS is needed for hosting the application locally or on a server.

- **.NET Hosting Bundle**
  Install the .NET Hosting Bundle to enable IIS to host ASP.NET Core applications.

---

## Installation Steps

### Step 1: Clone the Repository
Clone the MediCare repository from GitHub to your local machine:
```bash
git clone https://github.com/bohdansternytskyi7/MediCare.git
```

### Step 2: Configure the Database
Open MediCare solution in the Visual Studio. Build the solution → **Build** → **Build Solution**

### Step 3: Configure the Database
Generate the database schema by running migrations in "Package Manager Console":
```bash
update-database
```

After the database has been generated, you need to add a specific job to the database for maintaining appointment statuses:

- Create a job named **UpdateAppointmentStatusJob**.
- This job should execute the stored procedure **UpdateAppointmentStatus**.
- Schedule the job to run every 15 minutes.

### Step 3: Configure IIS for MediCare
#### 1. Create an Application Pool

- Open **IIS Manager**.
- In the left panel, click **Application Pools**.
- Right-click and select **Add Application Pool**.
  - Name: **MediCare AppPool**
  - .NET CLR Version: **No Managed Code**
  - Pipeline Mode: **Integrated**
- Click **OK**.
- Select newly created pool, right-click, select **Advanced Settings**, set Start Mode to **AlwaysRunning**

#### 2. Configure the Website in IIS

- In **IIS Manager**, right-click **Sites** and select **Add Website**.
  - Type: **https**
  - Host Name: **localhost**
  - Port: **5001**
- Click **OK**.

#### 3. Add Site Binding

- In **IIS Manager**, select **Default Web Site**, in the **Actions** panel showed on the right click **Bindings**. Add new binding:
  - Site Name: **MediCareWebApi**
  - Physical Path: *Specify the directory where the `MediCare.dll` file is located*
  - Application Pool: **MediCare AppPool**
- Click **OK**.

---

By following these steps, you will have the MediCare application up and running, ready to manage healthcare services efficiently!
