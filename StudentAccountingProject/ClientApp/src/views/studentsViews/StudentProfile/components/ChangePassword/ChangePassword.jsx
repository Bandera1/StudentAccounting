import React, { Component } from 'react';
import { Button } from 'primereact/button';
import { InputText } from 'primereact/inputtext';


class ChangePassword extends Component {
  state = {
    oldPassword: "",
    newPassword: "",
    confPassword: "",
    errorsState: {},
    errorServer: ''
  }

  onSubmitForm = (e) => {
    e.preventDefault();
    const { oldPassword, newPassword, confPassword } = this.state;
    let errorsState = {};
   
    if (oldPassword === '' || newPassword === '' || confPassword === '') {
      errorsState.oldPassword = "Enter value";
    }

    console.log(`new password ${newPassword} conf password ${confPassword}`);
    if (confPassword != newPassword) {
      errorsState.confPassword = "Passwords do not match";
    } else {
      errorsState.confPassword = "";
    }

    const isValid = Object.keys(errorsState).length === 0
    if (isValid) {
      const model = {
        oldPassword: oldPassword,
        newPassword: newPassword
      };

      // this.props.changePassword(model);
    }
    else {
      this.setState({ errorsState });
    }
  }


  setStateByErrors = (name, value) => {
    if (!!this.state.errorsState[name]) {
      let errorsState = Object.assign({}, this.state.errorsState);
      delete errorsState[name];
      this.setState(
        {
          [name]: value,
          errorsState
        }
      )
    }
    else {
      this.setState(
        { [name]: value })
    }
  }


  render() {
    const { errorsState, errorServer } = this.state;
    return (
      <form onBlur={this.onSubmitForm} >
        {!!errorServer ? <div>{errorServer}</div> : ""}
        <label className="p-float-label m-3 d-flex justify-content-center">Change password</label>
        <span className="p-float-label m-3">
          <InputText
            id="float-input"
            type="password"
            size="30"
            value={this.state.oldPassword}
            onChange={(e) => this.setState({ oldPassword: e.target.value })}
          />
          {!!errorsState.oldPassword ? <div style={{color:"red"}}>{errorsState.oldPassword}</div> : ""}
          <label htmlFor="float-input">Current password</label>
        </span>
        <span className="p-float-label  m-3">
          <InputText
            id="float-input"
            type="text"
            size="30"
            value={this.state.newPassword}
            onChange={(e) => this.setState({ newPassword: e.target.value })}
          />
          {!!errorsState.newPassword ? <div style={{ color: "red" }}>{errorsState.newPassword}</div> : ""}
          <label htmlFor="float-input">New password</label>
        </span>
        <span className="p-float-label  m-3">
          <InputText
            id="float-input"
            type="text"
            size="30"
            value={this.state.confPassword}
            onChange={(e) => this.setState({ confPassword: e.target.value })}
          />
          {!!errorsState.confPassword ? <div style={{ color: "red" }}>{errorsState.confPassword}</div> : ""}
          <label htmlFor="float-input">Confirm email</label>
        </span>
        {/* <Button className="p-float-label m-3" label="Change password" icon="pi pi-check" /> */}
      </form>
    );
  }
}

export default ChangePassword;