# Cypress Testing Guide for TaskForge.NET

## Overview

Cypress is an end-to-end testing framework ideal for testing modern web applications. This guide outlines how Cypress can be integrated into the TaskForge.NET project to test its web UI functionality.

**✨ New: The Cypress project now uses environment variables from `.env` file for all configuration - no hardcoded values!**

## What is TaskForge.NET?

TaskForge.NET is an ASP.NET Core MVC web application for project and task management with the following architecture:
- **Frontend**: ASP.NET Core MVC with Razor views
- **Backend**: ASP.NET Core Web API controllers
- **Authentication**: ASP.NET Core Identity
- **Database**: PostgreSQL with Entity Framework Core

---

## What Parts Can Cypress Test in TaskForge.NET?

### 1. **Authentication & Authorization Flows**

#### Testable Components:
- **User Registration** (`/Identity/Account/Register`)
  - Form validation
  - Successful user creation
  - Email confirmation flow
  - Error handling for duplicate users

- **User Login** (`/Identity/Account/Login`)
  - Valid credentials login
  - Invalid credentials error handling
  - Remember me functionality
  - Redirect after successful login
  - Two-factor authentication flow

- **User Logout** (`/Identity/Account/Logout`)
  - Successful logout
  - Session cleanup
  - Redirect to login page

- **Password Management**
  - Forgot password flow
  - Password reset with token
  - Password change from profile

- **Role-Based Access Control**
  - Admin-only features accessibility
  - User role restrictions
  - Viewer role limitations

---

### 2. **User Profile Management**

#### Testable Components:
- **Profile Setup** (`/UserProfile/Setup`)
  - First-time profile creation
  - Form field validation (Full Name, Phone, Location, Job Title, etc.)
  - Profile image upload
  - Professional summary input
  - LinkedIn and website URL validation

- **Profile Viewing** (`/UserProfile/Details`)
  - Display user information correctly
  - Show assigned projects
  - Display task statistics

---

### 3. **Project Management**

#### Testable Components:
- **Project List View** (`/Project/Index`)
  - Display all user projects
  - Filter projects by status (Not Started, In Progress, On Hold, Completed, Cancelled)
  - Search functionality
  - Pagination controls
  - Project card interactions

- **Project Creation** (`/Project/Create`)
  - Form validation (Title, Description, Status, Start/End dates)
  - Date picker functionality
  - Status dropdown selection
  - Successful project creation
  - Error handling

- **Project Dashboard** (`/Project/Dashboard/{id}`)
  - View project details (title, description, status, dates)
  - Task board with Kanban columns (To Do, In Progress, In Review, Done)
  - Drag-and-drop task movement between columns
  - Task filtering and sorting
  - Project statistics (total tasks, completed tasks, etc.)

- **Project Editing** (Modal in Dashboard)
  - Edit project title and description
  - Update project status
  - Modify start/end dates
  - Save changes validation
  - Permission checks (Admin only)

- **Member Management** (`/Project/ManageMembers`)
  - View project members
  - Add new members
  - Change member roles (Admin, Editor, Viewer)
  - Remove members
  - Role-based permissions

---

### 4. **Task Management**

#### Testable Components:
- **Task Creation** (Modal in Project Dashboard)
  - Form validation (Title, Description, Priority, Status)
  - Date picker for start and due dates
  - Priority selection (Low, Medium, High, Critical)
  - Status selection (To Do, In Progress, In Review, Done)
  - File attachment upload
  - Assign users to tasks
  - Task dependencies selection
  - Successful task creation

- **Task Viewing/Details** (Modal)
  - Display task information
  - Show attachments with download links
  - Display assigned users
  - View task dependencies
  - Show task status and priority

- **Task Editing** (Modal)
  - Update task fields
  - Add/remove attachments
  - Reassign users
  - Update dependencies
  - Change status and priority
  - Permission checks (Editor/Admin only)

- **Task Deletion**
  - Delete confirmation dialog
  - Successful deletion
  - Error handling for tasks with dependencies
  - Permission checks

- **Task Attachment Management**
  - Upload new attachments
  - Download existing attachments
  - Delete attachments
  - File type validation
  - File size validation

---

### 5. **Project Invitations**

#### Testable Components:
- **Send Invitation** (`/ProjectInvitation/Create`)
  - Enter recipient email
  - Select project
  - Define user role for invitee
  - Send invitation
  - Email notification trigger

- **Invitation List** (`/ProjectInvitation/Index`)
  - View pending invitations
  - View sent invitations
  - Pagination
  - Filter by status

- **Accept/Reject Invitation**
  - Click invitation link
  - Accept invitation flow
  - Reject invitation flow
  - Automatic project member addition

---

### 6. **Home Dashboard**

#### Testable Components:
- **User Dashboard** (`/Home/Index`)
  - Display total projects count
  - Display total tasks count
  - Display completed tasks count
  - Show user's task list
  - Task filtering
  - Pagination
  - Quick access to projects

- **Welcome Page** (Unauthenticated)
  - Landing page for non-logged-in users
  - Call-to-action buttons
  - Navigation to login/register

---

### 7. **User Management (Admin Only)**

#### Testable Components:
- **User List** (`/User/Index`)
  - Display all users
  - Search users by name/email
  - Filter by role (Admin, User, Operator)
  - Pagination
  - View user details

- **User Creation** (`/User/Create`)
  - Admin creating new users
  - Form validation
  - Role assignment
  - Email/username uniqueness check

- **User Deletion** (`/User/Delete`)
  - Soft delete functionality
  - Confirmation dialog
  - Admin permission check

---

### 8. **Navigation & UI Components**

#### Testable Components:
- **Top Navigation Bar**
  - Logo/brand link
  - Navigation menu items
  - User dropdown menu
  - Logout button
  - Responsive mobile menu

- **Breadcrumbs**
  - Navigation trail display
  - Clickable breadcrumb links

- **Modals**
  - Open/close functionality
  - Form submission within modals
  - Validation error display
  - Data persistence

- **Toast Notifications**
  - Success messages
  - Error messages
  - Warning messages
  - Auto-dismiss functionality

- **Responsive Design**
  - Mobile viewport testing
  - Tablet viewport testing
  - Desktop viewport testing
  - Touch interactions

---

### 9. **API Endpoints (Backend Testing)**

While Cypress primarily tests the UI, it can also test API endpoints directly:

- **Project API** (`/Project/*`)
  - GET, POST, PUT, DELETE operations
  - Response validation
  - Status codes

- **Task API** (`/Task/*`)
  - CRUD operations
  - File upload
  - JSON response validation

- **User API** (`/User/*`)
  - User CRUD operations

---

### 10. **Error Handling & Edge Cases**

#### Testable Scenarios:
- **Form Validation Errors**
  - Required field validation
  - Email format validation
  - Date range validation
  - File size/type validation

- **Network Errors**
  - API timeout handling
  - Server error responses (500, 503)
  - Retry mechanisms

- **Permission Errors**
  - Access denied pages
  - Unauthorized API calls
  - Role-based restrictions

- **Data Integrity**
  - Prevent circular task dependencies
  - Date logic validation (start date < end date)
  - Prevent duplicate project members

---

## Cypress Testing Setup for TaskForge.NET

### Prerequisites
- Node.js installed (v14 or higher)
- TaskForge.NET application running locally
- PostgreSQL database configured

### Project Structure

It's recommended to keep Cypress tests **separate from the backend code** in a dedicated testing folder. This approach:
- ✅ Keeps frontend tests isolated from backend code
- ✅ Makes it easier to manage test dependencies independently
- ✅ Allows different teams to work on tests without touching backend code
- ✅ Simplifies CI/CD pipeline configuration
- ✅ Follows industry best practices for E2E testing

**Recommended Structure:**
```
TaskForge.NET/
├── TaskForge.Application/
├── TaskForge.Domain/
├── TaskForge.Infrastructure/
├── TaskForge.Tests/          (Unit & Integration tests)
├── TaskForge.WebUI/
└── TaskForge.E2ETests/        (Cypress tests - NEW)
    ├── cypress/
    │   ├── e2e/
    │   ├── fixtures/
    │   └── support/
    ├── cypress.config.js
    ├── package.json
    └── README.md
```

### Installation Steps

1. **Navigate to the TaskForge.NET root directory:**
   ```powershell
   cd c:\Workstation\personal\brainstormers\TaskForge.NET
   ```

2. **Create a new E2E tests folder:**
   ```powershell
   mkdir TaskForge.E2ETests
   cd TaskForge.E2ETests
   ```

3. **Initialize npm:**
   ```powershell
   npm init -y
   ```

4. **Install Cypress:**
   ```powershell
   npm install cypress --save-dev
   ```

5. **Install additional helpful packages:**
   ```powershell
   npm install --save-dev @faker-js/faker cypress-file-upload
   ```

6. **Open Cypress for the first time:**
   ```powershell
   npx cypress open
   ```
   This creates the default Cypress folder structure.

7. **Cypress will create the following structure automatically:**
   ```
   TaskForge.E2ETests/
   ├── cypress/
   │   ├── e2e/              (Your test files go here)
   │   ├── fixtures/         (Test data JSON files)
   │   ├── support/          (Custom commands & global config)
   │   │   ├── commands.js
   │   │   └── e2e.js
   │   └── downloads/        (Downloaded files during tests)
   ├── cypress.config.js     (Main configuration)
   ├── package.json
   └── node_modules/
   ```

---

## Sample Cypress Test Examples

### Test Organization Structure
```
cypress/
├── e2e/
│   ├── auth/
│   │   ├── login.cy.js
│   │   ├── register.cy.js
│   │   └── password-reset.cy.js
│   ├── projects/
│   │   ├── create-project.cy.js
│   │   ├── edit-project.cy.js
│   │   └── project-dashboard.cy.js
│   ├── tasks/
│   │   ├── create-task.cy.js
│   │   ├── edit-task.cy.js
│   │   └── task-dependencies.cy.js
│   ├── user-profile/
│   │   └── profile-setup.cy.js
│   └── admin/
│       └── user-management.cy.js
├── fixtures/
│   ├── users.json
│   ├── projects.json
│   └── tasks.json
└── support/
    ├── commands.js
    └── e2e.js
```

---

### Example 1: User Login Test
```javascript
// cypress/e2e/auth/login.cy.js
describe('User Authentication', () => {
  beforeEach(() => {
    cy.visit('/Identity/Account/Login')
  })

  it('should successfully log in with valid credentials', () => {
    cy.get('input[name="Input.Email"]').type('testuser@example.com')
    cy.get('input[name="Input.Password"]').type('Password123!')
    cy.get('button[type="submit"]').click()

    // Assert redirection to home page
    cy.url().should('include', '/Home/Index')
    cy.contains('Welcome').should('be.visible')
  })

  it('should show error with invalid credentials', () => {
    cy.get('input[name="Input.Email"]').type('wrong@example.com')
    cy.get('input[name="Input.Password"]').type('WrongPassword')
    cy.get('button[type="submit"]').click()

    cy.contains('Invalid login attempt').should('be.visible')
  })
})
```

### Example 2: Project Creation Test
```javascript
// cypress/e2e/projects/create-project.cy.js
describe('Project Management', () => {
  beforeEach(() => {
    // Login first
    cy.login('testuser@example.com', 'Password123!')
    cy.visit('/Project/Create')
  })

  it('should create a new project successfully', () => {
    cy.get('input[name="Title"]').type('New Test Project')
    cy.get('textarea[name="Description"]').type('This is a test project description')
    cy.get('select[name="Status"]').select('1') // In Progress
    cy.get('input[name="StartDate"]').type('2025-11-01')
    cy.get('button[type="submit"]').click()

    // Assert success
    cy.url().should('include', '/Project/Index')
    cy.contains('New Test Project').should('be.visible')
  })

  it('should show validation error for empty title', () => {
    cy.get('textarea[name="Description"]').type('Description without title')
    cy.get('button[type="submit"]').click()

    cy.contains('Title is required').should('be.visible')
  })
})
```

### Example 3: Task Creation Test
```javascript
// cypress/e2e/tasks/create-task.cy.js
describe('Task Management', () => {
  beforeEach(() => {
    cy.login('testuser@example.com', 'Password123!')
    cy.visit('/Project/Dashboard/1') // Visit specific project
  })

  it('should create a new task', () => {
    cy.contains('button', 'Add Task').click() // Open modal

    cy.get('#createTaskModal').within(() => {
      cy.get('input[name="Title"]').type('New Test Task')
      cy.get('textarea[name="Description"]').type('Task description')
      cy.get('select[name="Priority"]').select('2') // High priority
      cy.get('select[name="Status"]').select('0') // To Do
      cy.get('button[type="submit"]').click()
    })

    // Assert task appears on board
    cy.contains('New Test Task').should('be.visible')
  })
})
```

### Example 4: API Testing
```javascript
// cypress/e2e/api/project-api.cy.js
describe('Project API', () => {
  let authToken

  before(() => {
    // Get authentication token
    cy.request({
      method: 'POST',
      url: '/Identity/Account/Login',
      form: true,
      body: {
        'Input.Email': 'testuser@example.com',
        'Input.Password': 'Password123!'
      }
    }).then((response) => {
      // Store token or cookies
    })
  })

  it('should return list of projects', () => {
    cy.request({
      method: 'GET',
      url: '/Project/Index',
      headers: {
        'Authorization': `Bearer ${authToken}`
      }
    }).then((response) => {
      expect(response.status).to.eq(200)
      expect(response.body).to.have.property('FilteredProjectList')
    })
  })
})
```

---

## Custom Commands

Create reusable commands in `cypress/support/commands.js`:

```javascript
// Login command
Cypress.Commands.add('login', (email, password) => {
  cy.session([email, password], () => {
    cy.visit('/Identity/Account/Login')
    cy.get('input[name="Input.Email"]').type(email)
    cy.get('input[name="Input.Password"]').type(password)
    cy.get('button[type="submit"]').click()
    cy.url().should('include', '/Home/Index')
  })
})

// Create project command
Cypress.Commands.add('createProject', (title, description) => {
  cy.visit('/Project/Create')
  cy.get('input[name="Title"]').type(title)
  cy.get('textarea[name="Description"]').type(description)
  cy.get('button[type="submit"]').click()
})
```

---

## Best Practices for TaskForge.NET

1. **Use Data Attributes**: Add `data-cy` attributes to important elements for more reliable selectors
   ```html
   <button data-cy="create-project-btn" type="submit">Create Project</button>
   ```

2. **Test Database Cleanup**: Reset test data between test runs
   ```javascript
   beforeEach(() => {
     cy.task('db:seed') // Seed test database
   })
   ```

3. **Mock API Responses**: Use `cy.intercept()` for predictable test scenarios
   ```javascript
   cy.intercept('GET', '/Project/Index*', { fixture: 'projects.json' })
   ```

4. **Test User Roles**: Create tests for each role (Admin, User, Operator, Viewer)

5. **Visual Testing**: Use Cypress snapshots or Percy for visual regression testing

6. **CI/CD Integration**: Run Cypress tests in GitHub Actions or Azure DevOps

---

## Running Cypress Tests

### Interactive Mode (Development)
```powershell
npx cypress open
```

### Headless Mode (CI/CD)
```powershell
npx cypress run
```

### Run Specific Test File
```powershell
npx cypress run --spec "cypress/e2e/auth/login.cy.js"
```

### Run with Specific Browser
```powershell
npx cypress run --browser chrome
```

---

## Test Coverage Recommendations

### Priority 1 (Critical Paths - Test First)
- User authentication (login/logout)
- Project creation and viewing
- Task creation and basic CRUD
- Role-based access control

### Priority 2 (Core Features)
- Project dashboard with Kanban board
- Task assignment and dependencies
- Project invitations
- User profile management

### Priority 3 (Secondary Features)
- File uploads and downloads
- Search and filtering
- Pagination
- Email notifications (mock)

---

## Getting Started

Ready to start testing? Follow these steps:

1. **Read the Quick Start Guide**: Check out [`TaskForge.E2ETests/QUICKSTART.md`](TaskForge.E2ETests/QUICKSTART.md) for a 5-minute setup guide

2. **Install Dependencies**:
   ```powershell
   cd TaskForge.E2ETests
   npm install
   ```

3. **Start Your Application**:
   ```powershell
   cd ..\TaskForge.WebUI
   dotnet run
   ```

4. **Run Tests**:
   ```powershell
   cd ..\TaskForge.E2ETests
   npm run cy:open
   ```

5. **Explore Example Tests**: Check the `cypress/e2e/` folder for ready-to-run test examples

---

## Project Files Created

The Cypress testing setup includes:

```
TaskForge.E2ETests/
├── cypress/
│   ├── e2e/
│   │   ├── auth/
│   │   │   ├── login.cy.js              ✅ Login tests
│   │   │   ├── register.cy.js           ✅ Registration tests
│   │   │   └── logout.cy.js             ✅ Logout tests
│   │   ├── projects/
│   │   │   ├── create-project.cy.js     ✅ Project creation tests
│   │   │   └── project-list.cy.js       ✅ Project list tests
│   │   ├── tasks/
│   │   │   └── create-task.cy.js        ✅ Task creation tests
│   │   └── user-profile/
│   │       └── profile-setup.cy.js      ✅ Profile setup tests
│   ├── fixtures/
│   │   ├── users.json                   ✅ Test user data
│   │   ├── projects.json                ✅ Sample projects
│   │   ├── tasks.json                   ✅ Sample tasks
│   │   └── user-profiles.json           ✅ Profile data
│   └── support/
│       ├── commands.js                  ✅ Custom commands
│       └── e2e.js                       ✅ Global config
├── cypress.config.js                    ✅ Cypress configuration
├── package.json                         ✅ NPM dependencies
├── .gitignore                          ✅ Git ignore rules
├── README.md                           ✅ Full documentation
└── QUICKSTART.md                       ✅ Quick start guide
```

---

## Conclusion

Cypress is an excellent fit for TaskForge.NET because:
- ✅ Tests real user interactions in the browser
- ✅ Can test both UI and API endpoints
- ✅ Provides fast, reliable, and flake-free tests
- ✅ Great debugging experience with time-travel
- ✅ Can test authentication flows and role-based access
- ✅ Supports file upload testing
- ✅ Can validate dynamic content (modals, toasts, drag-drop)

By implementing Cypress tests, you'll have confidence that critical user workflows in TaskForge.NET work correctly, catch bugs early, and maintain code quality as the application evolves.

**The Cypress project has been created in the `TaskForge.E2ETests` folder with:**
- ✅ Complete configuration files
- ✅ 8 example test files covering authentication, projects, and tasks
- ✅ Custom commands for common operations
- ✅ Test data fixtures
- ✅ Documentation and quick start guide

**Next Step**: Run `npm install` in the `TaskForge.E2ETests` folder to get started!

---

## Additional Resources

- [Cypress Documentation](https://docs.cypress.io/)
- [Cypress Best Practices](https://docs.cypress.io/guides/references/best-practices)
- [Testing ASP.NET Core Apps with Cypress](https://docs.cypress.io/guides/testing-strategies/asp-net-core)
- [TaskForge.E2ETests Quick Start Guide](TaskForge.E2ETests/QUICKSTART.md)
- [TaskForge.E2ETests README](TaskForge.E2ETests/README.md)
