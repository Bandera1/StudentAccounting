import React from 'react';


const Login = React.lazy(() => import("../views/othersViews/LoginPage"));

const routes = [
    { path: '/login', name: 'Login', component: Login },
];

export default routes;