Discussion Forum Web Application

#This project is a comprehensive implementation of a discussion forum web application, developed using ASP.NET Core 6.0 and the Model-View-Controller (MVC) framework. It is inspired by platforms like Stack Overflow, allowing users to ask and answer code-related questions.

## Project Description 
This web application serves as a discussion forum where users can ask questions and respond to others' queries, specifically focused on coding and technical topics. The key highlights of the project are:

- Users can browse and view questions and answers without logging in.
- Registered users can post questions, reply to questions, and edit or delete their own posts.
- Questions are categorized into one of ten distinct categories, making it easier to navigate and find relevant topics.
- The project aims to create a simple, user-friendly platform for technical discussions while maintaining a systematic approach to categorizing and displaying content.

## Technology Stack 
- ASP.NET Core 6.0: Backend framework MVC (Model-View-Controller) 
- Database management: SQL Server
- Frontend technologies: HTML/CSS/JavaScript

## How to Run the Project Locally

1. **Clone the repository**  
   ```bash
   git clone https://github.com/farhiamohamed/DiscussionForum-main.git
   ```

2. **Open the solution**  
   - Launch **Visual Studio**.
   - Open the solution file:  
     `DiscussionForum-main/DiscussionForum-main/DiscussionForum/DiscussionForum.sln`

3. **Configure the database**  
   - Create a local **SQL Server** database.
   - Run any `.sql` scripts provided to set up the schema (or manually create tables if required).
   - Update the connection string in `Web.config`:
   ```xml
   <connectionStrings>
     <add name="DefaultConnection" 
          connectionString="Data Source=localhost;Initial Catalog=DiscussionForumDB;Integrated Security=True"
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **Build and run the project**  
   - Press `F5` or click **Start Debugging** in Visual Studio.
   - The app should open in your default browser at `http://localhost:<port>/`.

## Contributors: 
- @mbhira7
- @farhiamohamed
