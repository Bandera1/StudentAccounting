import React from 'react';

const CourseTable = React.lazy(() => import("../views/studentsViews/CourseTable/CoursesSchedule"));
const AllCourses = React.lazy(() => import("../views/studentsViews/AllCorses/AllCourses"));
const StudentCourses = React.lazy(() => import("../views/studentsViews/MyCourses/MyCourses"));
const StudentProfile = React.lazy(() => import("../views/studentsViews/StudentProfile/StudentProfile"));

const routes = [
    { path: '/student/schedule', name: 'CoursesSchedule', component: CourseTable },
    { path: '/student/courses', name: 'AllCourses', component: AllCourses },
    { path: '/student/mycourses', name: 'StudentCourses', component: StudentCourses },
    { path: '/student/profile', name: 'StudentProfile', component: StudentProfile },
];

export default routes;