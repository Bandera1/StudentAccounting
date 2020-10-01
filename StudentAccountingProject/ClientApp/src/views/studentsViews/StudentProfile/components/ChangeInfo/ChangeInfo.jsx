import React, { Component } from 'react';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';
import { InputText } from 'primereact/inputtext';
import { Input } from 'reactstrap';


class ChangeInfo extends Component {
    state = {
        Name: "",
        SurName: "",
        Email: "",
        Age: 0
    }

    componentDidMount = () => {
        setTimeout(e => {
            let info = this.props.getInfoFromState();

            this.setState({
                Name: info.name,
                SurName: info.surname,
                Email: info.email,
                Age: info.age
            });
        },500)
        
    }

    onSubmitForm = (e) => {
        e.preventDefault();
        const { Name, SurName, Email, Age } = this.state;

        const model = {
            NewName: Name,
            NewSurName: SurName,
            NewEmail: Email,
            NewAge: Age
        };

        this.props.editStudent(model);
    }

    handleChange = e => {
        this.setState({ [e.target.name]: e.target.value });
    };


    render() {
        const { Name, SurName, Email, Age } = this.state;

        return (
            <form onSubmit={this.onSubmitForm}>
                <label className="p-float-label m-3 d-flex justify-content-center">Edit fields</label>
                <span className="p-float-label m-3">
                    <InputText id="float-input" name="Name" type="text" size="30" value={Name} onChange={this.handleChange} />
                    {Name.length <= 0 ? <label htmlFor="float-input">Name</label> : <div></div>}
                </span>
                <span className="p-float-label m-3">
                    <InputText id="float-input" name="SurName" type="text" size="30" value={SurName} onChange={this.handleChange} />
                    {SurName.length <= 0 ? <label htmlFor="float-input">Surname</label> : <div></div>}
                </span>
                <span className="p-float-label m-3">
                    <InputText id="float-input" name="Email" type="text" size="30" value={Email} onChange={this.handleChange} />
                    {Email.length <= 0 ? <label htmlFor="float-input">Email</label> : <div></div>}
                </span>
                <span className="p-float-label m-3">
                    <InputText id="float-input" name="Age" type="number" size="30" value={Age} onChange={this.handleChange} />
                    {Age.length <= 0 ? <label htmlFor="float-input">Age</label> : <div></div>}
                </span>
                <Button className="p-float-label m-3" label="Save info" icon="pi pi-check" />
            </form>
        );
    }
}

export default ChangeInfo;


