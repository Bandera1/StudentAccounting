import React, { Component } from 'react';
import ChangeImage from './components/ChangeImage/ChangeImage'
import ChangeInfo from './components/ChangeInfo/ChangeInfo'
import ChangePassword from './components/ChangePassword/ChangePassword'
import { Dimmer, Loader } from 'semantic-ui-react'
import {
    Card,
    CardGroup,
    Col,
    Container,
    Row,
} from "reactstrap";

import { connect } from "react-redux";
import get from "lodash.get";
import * as reducer from './reducer';

// import 'semantic-ui-css/semantic.min.css'

class StudentProfile extends Component {
    state = {
        info: {
            photoPath:'',
            name: '',
            surname: '',
            email: '',
            age: 0,
        },
        password: {
            currentPassword: '',
            oldPassword: ''
        },
        isLoading:false,
    }

    componentWillMount = () => {
        document.body.style.background = "#dcdde1"; 
        this.props.GetStudentInfo();
    };

    componentWillReceiveProps = (nextProps) => {
        console.log(nextProps);
        this.setState({
            info: nextProps.courseProfileReducer.getInfo.info,
            isLoading: nextProps.courseProfileReducer.getInfo.loading,
        });
    }

    getUserImage = () => {
        return this.state.info.photoPath === undefined ? '' : this.state.info.photoPath;
    }

    getInfoFromState = () => {
        return this.state.info === undefined ? undefined : this.state.info;
    }

    editStudent = (model) => {

    }

    render() {
        const { isLoading } = this.state;

        return (
            <Container>
                {isLoading && <Dimmer active>
                    <Loader style={{ maxHeight: "100vh" }}>Loading</Loader>
                </Dimmer>}
                <Row className="justify-content-center mt-5">
                    <Card>
                        <Row className="justify-content-center mt-5">
                            <Col>
                                <CardGroup>
                                    <ChangeImage getUserImage={this.getUserImage}/>
                                </CardGroup>
                            </Col>
                            <Col>
                                <CardGroup>
                                    <ChangePassword />
                                </CardGroup>
                            </Col>
                            <Col>
                                <CardGroup>
                                    <ChangeInfo getInfoFromState={this.getInfoFromState} editStudent={this.editStudent}/>
                                </CardGroup>
                            </Col>
                        </Row>
                    </Card>
                </Row>
            </Container>
        );
    }
}

// 2
// GetReducerData
function mapStateToProps(state) {
    return {
        courseProfileReducer: get(state, 'StudentProfile'),
    };
}


//1
//Call reducer
const mapDispatch = (dispatch) => {
    return {
        GetStudentInfo: () => {
            dispatch(reducer.GetStudentInfo());
        }
    };
};


export default connect(mapStateToProps, mapDispatch)(StudentProfile);