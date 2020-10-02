import React, { Component } from 'react';
import ChangeImage from './components/ChangeImage/ChangeImage'
import ChangeInfo from './components/ChangeInfo/ChangeInfo'
import ChangePassword from './components/ChangePassword/ChangePassword'
import { Toast } from 'primereact/toast';
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
            photoPath: '',
            name: '',
            surname: '',
            email: '',
            age: 0,
        },
        password: {
            currentPassword: '',
            newPassword: ''
        },
        newPhotoBase64: '',

        isPasswordError: false,
        isFieldsError: false,

        isPasswordSuccess: false,
        isFieldsSuccess: false,

        passwordError: '',
        fieldsError: ''
    }

    componentWillMount = () => {
        document.body.style.background = "#dcdde1";
        this.props.GetStudentInfo();
    };

    componentWillReceiveProps = (nextProps) => {
        console.log(nextProps);
        this.setState({
            info: nextProps.courseProfileReducer.getInfo.info,

            isPasswordError: nextProps.courseProfileReducer.updatePassword.failed,
            isFieldsError: nextProps.courseProfileReducer.updateStudent.failed,

            isPasswordSuccess: nextProps.courseProfileReducer.updatePassword.success,
            isFieldsSuccess: nextProps.courseProfileReducer.updateStudent.success,

            passwordError: nextProps.courseProfileReducer.updatePassword.error,
            fieldsError: nextProps.courseProfileReducer.updateStudent.error,
        });
    }

    getUserImage = () => {
        return this.state.info.photoPath === undefined ? '' : this.state.info.photoPath;
    }

    changeImage = (img) => {
        this.setState({ newPhotoBase64: img });
    }

    getInfoFromState = () => {
        return this.state.info === undefined ? undefined : this.state.info;
    }

    editStudent = (model) => {
        const { info,newPhotoBase64 } = this.state;

        let newModel = {
            dto: {
                name: model.NewName,
                surname: model.NewSurName,
                age: model.NewAge.toString(),
                email: model.NewEmail
            }
        }

        if (newPhotoBase64 != '') {
            let newImageModel = {
                Model: {
                    imageBase64: newPhotoBase64
                }
            }
            this.props.UpdateImage(newImageModel);
        }

        if (newModel.name == ''
            && newModel.surname == ''
            && newModel.email == ''
            && newModel.age == 0) {
            return 0;
        }

        if (newModel.name == info.name
            && newModel.surname == info.surname
            && newModel.email == info.email
            && newModel.age == info.age) {
            return 0;
        } else {
            console.log(newModel);
            this.props.UpdateStudent(newModel);
        }
        
    }

    changePassword = (model) => {
        if (model.oldPassword != '' && model.newPassword != '') {
            this.setState({
                password: {
                    currentPassword: model.oldPassword,
                    newPassword: model.newPassword
                }
            })
        }
    }


    ChangePasswordSuccessGrowl = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'success', summary: 'Success', detail: 'Password has changed', life: 3000 });
        setTimeout(() => { window.location.reload(); }, 1500);
    }

    ChangePasswordFailedGrowl = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'error', summary: 'Failed', detail: this.state.passwordError, sticky: true });
    }

    ChangeDataSuccessGrowl = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'success', summary: 'Success', detail: 'Data has changed', life: 3000 });
        setTimeout(() => { window.location.reload(); }, 1500);
    }

    ChangeDataFailedGrowl = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'error', summary: 'Failed', detail: this.state.fieldsError, sticky: true });
    }

    render() {
        const { isPasswordError, isFieldsError, isPasswordSuccess, isFieldsSuccess } = this.state;

        if (isPasswordError) {
            this.ChangePasswordFailedGrowl();
        } else if (isPasswordSuccess) {
            this.ChangePasswordSuccessGrowl();
        }
        if (isFieldsError) {
            this.ChangeDataFailedGrowl();
        } else if (isFieldsSuccess) {
            this.ChangeDataSuccessGrowl();
        }

        return (
            <React.Fragment>              
                <Container>
                    <Toast ref={(el) => this.toast = el} />
                    <Row className="justify-content-center mt-5">
                        <Card>
                            <Row className="justify-content-center mt-5">
                                <Col>
                                    <CardGroup>
                                        <ChangeImage getUserImage={this.getUserImage} changeImage={this.changeImage} />
                                    </CardGroup>
                                </Col>
                                {
                                    !this.state.info.isFacebookAccount && this.state.info.isFacebookAccount !== undefined ?
                                        <Col>
                                            <CardGroup>
                                                <ChangePassword changePassword={this.changePassword} />
                                            </CardGroup>
                                        </Col> :
                                        <div></div>
                                }                            
                                <Col>
                                    <CardGroup>
                                        <ChangeInfo getInfoFromState={this.getInfoFromState} editStudent={this.editStudent} />
                                    </CardGroup>
                                </Col>
                            </Row>
                        </Card>
                    </Row>
                </Container>
            </React.Fragment>
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
        },
        UpdateStudent: (model) => {
            dispatch(reducer.UpdateStudent(model));
        },
        UpdatePassword: (model) => {
            dispatch(reducer.UpdatePassword(model));
        },
        UpdateImage: (model) => {
            dispatch(reducer.UpdateImage(model));
        },
        ClearState: () => {
            dispatch(reducer.ClearState());
        }
    };
};


export default connect(mapStateToProps, mapDispatch)(StudentProfile);