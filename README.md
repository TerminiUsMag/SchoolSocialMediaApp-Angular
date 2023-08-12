# School Social Media App Project.

This is a school social media web application I'm developing for SoftUni's "ASP.NET Advanced" course.

[Link to the course page](https://softuni.bg/trainings/4107/asp-net-advanced-june-2023).
---------------------------------------------------------------------------------------------------------------------
Requirements for the project are added in folder "Project Requirements".

First Run:  
On the first run the application will seed: 
<ul>
  <li>Admin User: Can Manage the whole application (Delete users, manage schools, send invitations, delete and edit posts, delete comments, change school pictures and more).</li>
  <li>Principal User: Has demo school and a post in the school. Can Manage the whole school.(Send invitations, remove users from school, manage posts and comments).</li>
  <li>Student User: Has invitation from the principal to join the demo school as a student, commented and liked the Principal's post. Can post and comment in the school which he is part of, can quit the school and can receive invitations from principals and admins to join schools as a concreate role.</li>
  <li>Parent User: Can be added to the school as parent.  </li>
  <li>Teacher User: Can be added to the school as teacher. </li>
</ul>
 
Main Functionalities:
<ol>
  <li>Users Login and Register.</li>
  <li>After registration you can register your school (You become the "principal" of the school).</li>
  <li>The Principal can send invitations to Non-Principal users to be teachers, parents or students in his school.</li>
  <li>Invited Users receive the principal's invitation and can choose to accept or decline the invitation.</li>
  <li>At any time the user can quit the school he is part of.</li>
  <li>As a part of school you have access to all posts related to the school you are part of.</li>
  <li>As a part of school you can like and comment each post related to your school.</li>
  <li>If you are the creator of a post you can edit and delete it.</li>
  <li>If you are the principal of a school you can edit and delete every post related to the school.</li>
  <li>The admin can edit or delete every post related to every school and can delete whole schools and users.</li>
  <li>You can edit your profile, change your profile picture.</li>
  <li>If you are a principal you can edit your school and change it's profile picture.</li>
  <li>Admin Panel.</li>
  <li>As admin you can do everything the principal can for every registered school.(Excluding: Post Creation).</li>
  <li>The admin can delete users.</li>
</ol>
