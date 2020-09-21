import React from 'react';

const AllStudents = React.lazy(() => import("../views/adminViews/AllStudents/AllStudents"));


const routes = [
    { path: '/admin/students', name: 'AdminStudents', component: AllStudents },
];

export default routes;