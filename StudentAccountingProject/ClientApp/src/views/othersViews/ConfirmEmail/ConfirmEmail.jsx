import React, { Component } from 'react';
import history from "../../../utils/history";

import { connect } from "react-redux";
import get from "lodash.get";
import * as reducer from "./reducer";

class ConfirmEmail extends Component {
    state = 
    { 
        userId:""
    }

    componentDidMount() {
        let tmp = this.props.match.params.id;
        if(tmp === undefined)
        {
            history.push("/#/");
            history.go();
        }
        let id = tmp.split("=").splice(1, 1).toString();
        this.setState({ userId: id });

        let model = {
            dto:{
                userId:id
            }
        }
        this.props.ConfirmEmail(model);
        setTimeout(() => {
            history.push("/#/student/schedule");
            history.go();
        },4000);
    }   


    render() { 
        return (<div>You have verified your mail. You will now be redirected.</div> );
    }
}
 
// 2
// GetReducerData
function mapStateToProps(state) {
    return {
        confirmEmailReducer: get(state, 'confirmEmail'),
    };
}


//1
//Call reducer
const mapDispatch = (dispatch) => {
    return {
        ConfirmEmail: (model) => {
            dispatch(reducer.ConfirmEmail(model));
        }
    };
};


export default connect(mapStateToProps, mapDispatch)(ConfirmEmail);