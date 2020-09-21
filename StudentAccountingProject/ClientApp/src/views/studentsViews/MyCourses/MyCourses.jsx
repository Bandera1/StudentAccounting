import React, { Component } from 'react';
import { Card } from 'primereact/card';
import { Rating } from 'primereact/rating';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faCalendar } from '@fortawesome/free-solid-svg-icons'
import { Dimmer, Loader } from 'semantic-ui-react'

import { connect } from "react-redux";
import get from "lodash.get";
import * as reducer from './reducer';

import 'semantic-ui-css/semantic.min.css'
import 'primereact/resources/themes/bootstrap4-light-blue/theme.css'
import '../AllCorses/styles.css'

class MyCourses extends Component {
    state = { 
        courses:[],
        isLoading:false,
     }


    componentWillMount = () => {
        this.props.GetStudentsCourses();
    };

    componentWillReceiveProps = (nextProps) => {
        this.setState({
            courses: nextProps.studentsCoursesReducer.courses,
            isLoading: nextProps.studentsCoursesReducer.post.loading,
        });
    }

    render() { 
        const { isLoading,courses } = this.state;

        const header = (
            <img style={{ borderRadius: "0.3rem" }} alt="Card" src='https://elearning-reskill.eu/pluginfile.php/99/course/overviewfiles/products-online-courses.png' />
        );

        return (
            <div>
                {isLoading && <Dimmer active>
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
                                    // footer={footer(item.id, item.isSubscribe)}
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
        studentsCoursesReducer: get(state, 'studentsCourses'),
    };
}


//1
//Call reducer
const mapDispatch = (dispatch) => {
    return {
        GetStudentsCourses: () => {
            dispatch(reducer.GetStudentsCourses());
        }
    };
};


export default connect(mapStateToProps, mapDispatch)(MyCourses);