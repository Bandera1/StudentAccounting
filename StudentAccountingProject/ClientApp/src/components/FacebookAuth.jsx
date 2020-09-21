import React, { Component } from 'react';
import FacebookLogin from 'react-facebook-login';

class FacebookAuth extends Component {
    state = {
        isLoggedIn: false,
        userId: '',
        name: '',
        email: '',
    }
    
    responseFacebook = response => {
        console.log("Response");
        console.log(response);
        let model = {
            dto:{
                name:response.first_name,
                surname:response.last_name,
                email:response.email
            }
        }
        this.props.login(model);
    }

    componentClicked = () => {console.log('Clicked');}

    render() {
        let fbContent;

        if(this.state.isLoggedIn) {
            fbContent = null;
        } else {
            fbContent = (<FacebookLogin
                appId="1204392603275627"
                autoLoad={false}
                fields="first_name,last_name,email,birthday"
                onClick={this.componentClicked}
                callback={this.responseFacebook} />);
        }

        return (
        <div>{fbContent}</div>
        );
    }
}

export default FacebookAuth;