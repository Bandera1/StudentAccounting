import React, { Component } from 'react';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';
import { Rating } from 'primereact/rating';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faCalendar } from '@fortawesome/free-solid-svg-icons'
import { Dimmer, Loader } from 'semantic-ui-react'
import { Toast } from 'primereact/toast';

import { connect } from "react-redux";
import get from "lodash.get";
import * as reducer from "./reducer";

import 'semantic-ui-css/semantic.min.css'
import 'primereact/resources/themes/bootstrap4-light-blue/theme.css'
import './styles.css'


class AllCourses extends Component {
    state = {
        courses: [],
        isLoading:false,
        isSubscribeLoading:false,
        isSuccess:false,
        isFailed:false,
        errorMessage:""
    }

    componentWillMount = () => {
        this.props.GetAllCourses();
    };

    componentWillReceiveProps = (nextProps) => {
        this.setState({
            courses: nextProps.GetAllCoursesReducer.courses,
            isLoading: nextProps.GetAllCoursesReducer.subscribePost.loading,
            isSubscribeLoading: nextProps.GetAllCoursesReducer.subscribePost.loading,
            isSuccess: nextProps.GetAllCoursesReducer.subscribePost.success,
            isFailed: nextProps.GetAllCoursesReducer.subscribePost.failed,
            errorMessage: nextProps.GetAllCoursesReducer.subscribePost.error
        });
    }

    subcribeToCourse = (id) => {
        console.log("Course id",id)
        let myModel = {
            model:{
                courseId:id,
                studentId:""
            }
        }
        console.log(myModel);
        this.props.SubscribeToCourse(myModel);
    }

    showSuccessGrowl = () => {
        this.setState({ isSuccess: false });
        this.toast.show({ severity: 'success', summary: 'Success', detail: 'You subscribe to course', life: 3000 });
    }

    showFailedGrowl = (error) => {
        this.setState({ isFailed: false });
        this.toast.show({ severity: 'error', summary: 'Failed', detail: error, sticky: true });
    }

    render() {
        const { courses, isLoading, isSubscribeLoading, isSuccess, isFailed, errorMessage } = this.state;

        const header = (
            <img style={{ borderRadius: "0.3rem" }} alt="Card" src='https://elearning-reskill.eu/pluginfile.php/99/course/overviewfiles/products-online-courses.png' />
        );
        const footer = (key,isSubscribe) => (
            <span>
                {!isSubscribe ?
                    <Button label="Subscribe" className="p-button-success" onClick={e => this.subcribeToCourse(key)} />
                    : <Button label="You are already subscribed" className="p-button-success" icon="pi pi-check" disabled />}
            </span>
        );

        if (isSuccess) {
            this.showSuccessGrowl();
            setTimeout(() => { window.location.reload(); }, 2000);
        }

        if (isFailed) {
            this.showFailedGrowl(errorMessage);
        }

        return (
            <div>
                <Toast ref={(el) => this.toast = el} />
                {isLoading || isSubscribeLoading  && <Dimmer active>
                    <Loader style={{ maxHeight: "100vh" }}>Loading</Loader>
                </Dimmer>}
                <div className="container mt-2">
                    {

                        courses != undefined ?
                            courses.map(item => {
                                return (<Card
                                    key={item.id}
                                    title={item.name}
                                    subTitle={item.author}
                                    style={
                                        {
                                            backgroundColor: "white",
                                            width: '25rem',
                                            marginBottom: '2em',
                                            boxShadow: "5px 3px 13px -3px rgba(0,0,0,0.66)"
                                        }}
                                    footer={footer(item.id,item.isSubscribe)}
                                    header={header}>
                                    <p className="p-m-0 " style={{ lineHeight: '1.5' }}>{item.description}</p>
                                    <p className="p-m-0 " style={{ lineHeight: '1.5' }}>
                                        <FontAwesomeIcon icon={faCalendar} />
                                        &nbsp;
                                        {item.dateOfStart}
                                         &nbsp;
                                        -
                                         &nbsp;
                                        {item.dateOfEnd}
                                    </p>
                                    <Rating value={item.rating} readonly stars={5} cancel={false} />
                                </Card>)
                            }) : <div></div>
                    }
                </div>
            </div>
        );
    }
}


// 2
// GetReducerData
function mapStateToProps(state) {
    return {
        GetAllCoursesReducer: get(state, 'getAllCourses'),
    };
}


//1
//Call reducer
const mapDispatch = (dispatch) => {
    return {
        GetAllCourses: () => {
            dispatch(reducer.GetAllCourses());
        },
        SubscribeToCourse: (model) => {
            dispatch(reducer.SubscribeToCourse(model));
        }
    };
};

export default connect(mapStateToProps, mapDispatch)(AllCourses);