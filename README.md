# School Social Media App Project.

This is a school social media web application I'm developing for SoftUni's "ASP.NET Advanced" course.

[Link to the course page](https://softuni.bg/trainings/4107/asp-net-advanced-june-2023).
---------------------------------------------------------------------------------------------------------------------
Requirements for the project are added in folder "Project Requirements".

First Run:  
On the first run the application will seed:   
Admin User: Can Manage the whole application (Delete users, manage schools, send invitations, delete and edit posts, delete comments, change school pictures and more).  
Principal User: Has demo school and a post in the school. Can Manage the whole school.(Send invitations, remove users from school, manage posts and comments).  
Student User: Has invitation from the principal to join the demo school as a student, commented and liked the Principal's post. Can post and comment in the school which he is part of, can quit the school and can receive invitations from principals and admins to join schools as a concreate role.  
Parent User: Can be added to the school as parent.   
Teacher User: Can be added to the school as teacher.  

Main Functionalities:
1. Users Login and Register.
2. After registration you can register your school (You become the "principal" of the school).
3. The Principal can send invitations to Non-Principal users to be teachers, parents or students in his school.
4. Invited Users receive the principal's invitation and can choose to accept or decline the invitation.
5. At any time the user can quit the school he is part of.
6. As a part of school you have access to all posts related to the school you are part of.
7. As a part of school you can like and comment each post related to your school.
8. If you are the creator of a post you can edit and delete it.
9. If you are the principal of a school you can edit and delete every post related to the school.
10. The admin can edit or delete every post related to every school and can delete whole schools and users.
11. You can edit your profile, change your profile picture.
12. If you are a principal you can edit your school and change it's profile picture.
13. Admin Panel.
14. As admin you can do everything the principal can for every registered school.(Excluding: Post Creation).
15. The admin can delete users.
