import React from 'react';

const AllStudents = React.lazy(() => import("../views/adminViews/AllStudents/AllStudents"));
const AllCourses = React.lazy(() => import("../views/adminViews/AllCourses/AllCourses"));


const routes = [
    { path: '/admin/students', name: 'AdminStudents', component: AllStudents },
    { path: '/admin/courses', name: 'AdminCourses', component: AllCourses },
];

export default routes;