import React, { Component } from 'react';
import classNames from 'classnames';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Toast } from 'primereact/toast';
import { Button } from 'primereact/button';
import { Toolbar } from 'primereact/toolbar';
import { InputNumber } from 'primereact/inputnumber';
import { Dialog } from 'primereact/dialog';
import { InputText } from 'primereact/inputtext';
import { Dimmer, Loader } from 'semantic-ui-react'
import 'semantic-ui-css/semantic.min.css'

import get from "lodash.get";
import { connect } from "react-redux";
import * as reducer from './reducer';


import './style.scss'
import 'primereact/resources/themes/saga-blue/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';


class AllStudents extends Component {
    emptyStudent = {
        hashId: null,
        id: null,
        name: '',
        surname:'',
        age: 1,
        email: '',
        registerDate: '',     
        dateOfCourseStart:''     
    };


    constructor(props) {
        super(props);

        this.state = {
            Students: [],
            StudentDialog: false,
            deleteStudentDialog: false,
            deleteStudentsDialog: false,
            Student: this.emptyStudent,
            selectedStudents: null,
            submitted: false,
            globalFilter: null,
            isLoadingPage:false,

            loading:false,
            first: 0,
            totalRecords: 0,
            rows:4,

            isStudentCreateSuccess: false,
            isStudentEditSuccess: false,
            isStudentDeleteSuccess: false,

            isStudentCreateFailed: false,
            isStudentEditFailed: false,
            isStudentDeleteFailed: false,

            createStudentErrorMessage: '',
            editStudentErrorMessage: '',
            deleteStudentErrorMessage: '',
        };

        this.leftToolbarTemplate = this.leftToolbarTemplate.bind(this);
        this.rightToolbarTemplate = this.rightToolbarTemplate.bind(this);
        this.emailBodyTemplate = this.emailBodyTemplate.bind(this);
        this.actionBodyTemplate = this.actionBodyTemplate.bind(this);

        this.openNew = this.openNew.bind(this);
        this.hideDialog = this.hideDialog.bind(this);
        this.saveStudent = this.saveStudent.bind(this);
        this.editStudent = this.editStudent.bind(this);
        this.confirmDeleteStudent = this.confirmDeleteStudent.bind(this);
        this.deleteStudent = this.deleteStudent.bind(this);
        this.exportCSV = this.exportCSV.bind(this);
        this.confirmDeleteSelected = this.confirmDeleteSelected.bind(this);
        this.deleteSelectedStudents = this.deleteSelectedStudents.bind(this);
        this.onAgeChange = this.onAgeChange.bind(this);
        this.onInputChange = this.onInputChange.bind(this);
        this.onInputNumberChange = this.onInputNumberChange.bind(this);
        this.hideDeleteStudentDialog = this.hideDeleteStudentDialog.bind(this);
        this.hideDeleteStudentsDialog = this.hideDeleteStudentsDialog.bind(this);
    }


    componentDidMount = () => {
        this.props.GetStudentsCount();
        setTimeout(x => { this.initStudents();},1000)
    };

    componentWillReceiveProps = (nextProps) => {
        //- Binding
        console.log("Next props",nextProps);
        this.setState({
            Students: nextProps.studentReducer.getStudents.students,
            loading: nextProps.studentReducer.getStudents.loading,

            totalRecords: nextProps.studentReducer.getCount.count,

            //------StudentCreate
            isStudentCreateSuccess: nextProps.studentReducer.createStudent.success,
            isStudentCreateFailed: nextProps.studentReducer.createStudent.failed,
            //------StudentEdit
            isStudentEditSuccess: nextProps.studentReducer.editStudent.success,
            isStudentEditFailed: nextProps.studentReducer.editStudent.failed,
            //------StudentDelete
            isStudentDeleteSuccess: nextProps.studentReducer.deleteStudent.success,
            isStudentDeleteFailed: nextProps.studentReducer.deleteStudent.failed,

            //------Errors
            createStudentErrorMessage: nextProps.studentReducer.createStudent.error,
            editStudentErrorMessage: nextProps.studentReducer.editStudent.error,
            deleteStudentErrorMessage: nextProps.studentReducer.deleteStudent.error,
        });
    }

    initStudents = () => {
        const {first,rows} = this.state;
        if(this.state.totalRecords >= this.state.rows)
        {
            this.updateStudents(first,first+rows);
        } else {
            this.updateStudents(0, this.state.totalRecords);
        }
    }

    updateStudents = (from,to) => {
        this.props.GetAllStudents(from, to);
    }

    openNew() {
        console.log("open");
        this.setState({
            Student: this.emptyStudent,
            submitted: false,
            StudentDialog: true
        });
    }

    hideDialog() {
        this.setState({
            submitted: false,
            StudentDialog: false
        });
    }

    hideDeleteStudentDialog() {
        this.setState({ deleteStudentDialog: false });
    }

    hideDeleteStudentsDialog() {
        this.setState({ deleteStudentsDialog: false });
    }

    saveStudent() {
        console.log("Save student");
        let state = { submitted: true };

        if (this.state.Student.name.trim()) {
            let Student = { ...this.state.Student };
            if (this.state.Student.id) {               
                let editedStudent = {
                    DTO: {
                        id: Student.hashId,
                        Name: Student.name,
                        Surname: Student.surname,
                        Age: Student.age.toString(),
                        Email: Student.email
                    }
                };
                console.log("Update student", editedStudent);
                this.props.EditStudent(editedStudent);
            }
            else {
                let newStudent = {
                    DTO: {
                        name: Student.name,
                        surname: Student.surname,
                        age: Student.age.toString(),
                        email: Student.email
                    }
                }
                console.log("Create student", newStudent);
                this.props.CreateStudent(newStudent);               
            }

            state = {
                ...state,
                StudentDialog: false,
                Student: this.emptyStudent
            };
        }

        this.setState(state);
    }

    editStudent(Student) {
        this.setState({
            Student: { ...Student },
            StudentDialog: true
        });
    }

    confirmDeleteStudent(Student) {
        this.setState({
            Student,
            deleteStudentDialog: true
        });
    }

    deleteStudent() {
        let deleteStudent = this.state.Students.filter(val => val.id === this.state.Student.id);
        let model = {
            DTO:{
                students: [{
                    studentId: deleteStudent[0].hashId            
                }]
            }
        }
        console.log("Delete val", model);
        this.props.DeleteStudent(model);

        this.setState({            
            deleteStudentDialog: false,
            product: this.emptyStudent
        });
    }

    findIndexById(id) {
        let index = -1;
        for (let i = 0; i < this.state.Students.length; i++) {
            if (this.state.Students[i].id === id) {
                index = i;
                break;
            }
        }

        return index;
    }

    createId() {
        let id = '';
        let chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        for (let i = 0; i < 5; i++) {
            id += chars.charAt(Math.floor(Math.random() * chars.length));
        }
        return id;
    }

    exportCSV() {
        this.dt.exportCSV();
    }

    confirmDeleteSelected() {
        this.setState({ deleteStudentsDialog: true });
    }

    deleteSelectedStudents() {
        let students = this.state.Students.filter(val => this.state.selectedStudents.includes(val)).map(a => {
            return {
                studentId: a.hashId
            }
        });
        let model = {
            DTO:{
                students:[
                    ...students
                ] 
            }
        }
        
        console.log("Deleted students", model);
        this.props.DeleteStudent(model);

        this.setState({          
            deleteStudentsDialog: false,
            selectedStudents: null
        });
    }

    onAgeChange(e) {
        let Student = { ...this.state.Student };
        Student['age'] = e.value;
        this.setState({ Student });
    }

    onInputChange(e, name) {
        const val = (e.target && e.target.value) || '';
        let Student = { ...this.state.Student };
        Student[`${name}`] = val;

        this.setState({ Student });
    }

    onInputNumberChange(e, name) {
        const val = e.value || 0;
        let Student = { ...this.state.Student };
        Student[`${name}`] = val;

        this.setState({ Student });
    }

    leftToolbarTemplate() {
        return (
            <>
                <Button label="New" icon="pi pi-plus" className="p-button-success p-mr-2 mr-3" onClick={this.openNew} />
                <Button label="Delete" icon="pi pi-trash" className="p-button-danger" onClick={this.confirmDeleteSelected} disabled={!this.state.selectedStudents || !this.state.selectedStudents.length} />
            </>
        )
    }

    rightToolbarTemplate() {
        return (
            <>
                <Button label="Export" icon="pi pi-upload" className="p-button-help" onClick={this.exportCSV} />
            </>
        )
    }

    emailBodyTemplate(rowData) {
        return <span className="Student-badge status-instock">{rowData.email}</span>;
    }

    actionBodyTemplate(rowData) {
        return (
            <>
                <Button icon="pi pi-pencil" className="p-button-rounded p-button-success p-mr-2 mr-2" onClick={() => this.editStudent(rowData)} />
                <Button icon="pi pi-trash" className="p-button-rounded p-button-warning" onClick={() => this.confirmDeleteStudent(rowData)} />
            </>
        );
    }

    onPage(event){       
        const { first, rows } = event;
        const {totalRecords} = this.state;

        console.log(`From ${first} To ${first + (totalRecords - first)}`);
        this.updateStudents(first, first + (totalRecords - first));
        this.setState({first});
    }

//-----------------------TOASTS------------------------------
    //-------CreateStudent
    createStudentSuccessToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'success', summary: 'Successful', detail: 'Student Created', life: 3000 });
        window.location.reload();
    }

    createStudentFailedToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'error', summary: 'Failed', detail: this.state.createStudentErrorMessage, life: 3000 });
    }

    //-------EditStudent
    editStudentSuccessToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'success', summary: 'Successful', detail: 'Student updated', life: 3000 });
        window.location.reload();
    }
    editStudentFailedToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'error', summary: 'Failed', detail: this.state.editStudentErrorMessage, life: 3000 });
    }

    //-------DeleteStudent
    deleteStudentSuccessToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'success', summary: 'Successful', detail: 'Deleted', life: 3000 });
        window.location.reload();
    }
    deleteStudentFailedToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'error', summary: 'Failed', detail: this.state.deleteStudentErrorMessage, life: 3000 });
    }

//-----------------------------------------------------------

    render() { 
        const header = (
            <div className="table-header">
                <h5 className="p-m-0">Students</h5>
                <span className="p-input-icon-left">
                    <i className="pi pi-search" />
                    <InputText type="search" onInput={(e) => this.setState({ globalFilter: e.target.value })} placeholder="Search..." />
                </span>
            </div>
        );
        const StudentDialogFooter = (
            <>
                <Button label="Cancel" icon="pi pi-times" className="p-button-text" onClick={this.hideDialog} />
                <Button label="Save" icon="pi pi-check" className="p-button-text" onClick={this.saveStudent} />
            </>
        );
        const deleteStudentDialogFooter = (
            <>
                <Button label="No" icon="pi pi-times" className="p-button-text" onClick={this.hideDeleteStudentDialog} />
                <Button label="Yes" icon="pi pi-check" className="p-button-text" onClick={this.deleteStudent} />
            </>
        );
        const deleteStudentsDialogFooter = (
            <>
                <Button label="No" icon="pi pi-times" className="p-button-text" onClick={this.hideDeleteStudentsDialog} />
                <Button label="Yes" icon="pi pi-check" className="p-button-text" onClick={this.deleteSelectedStudents} />
            </>
        );
        
        if (this.state.isStudentCreateSuccess) this.createStudentSuccessToast();
        if (this.state.isStudentCreateFailed ) this.createStudentFailedToast();
        if (this.state.isStudentEditSuccess) this.editStudentSuccessToast();
        if (this.state.isStudentEditFailed) this.editStudentFailedToast();
        if (this.state.isStudentDeleteSuccess) this.deleteStudentSuccessToast();
        if (this.state.isStudentDeleteFailed) this.deleteStudentFailedToast();

        return (       
            <div className="datatable-crud-demo p-3">
                {/* {this.state.isLoadingPage && <Dimmer active>
                    <Loader style={{ maxHeight: "100vh" }}>Loading</Loader>
                </Dimmer>} */}
                <Toast ref={(el) => this.toast = el} />

                <div className="card">
                    <Toolbar className="p-mb-4" left={this.leftToolbarTemplate} right={this.rightToolbarTemplate}></Toolbar>

                    <DataTable ref={(el) => this.dt = el} 
                        loading={this.state.loading}
                        lazy first={this.state.first}
                        totalRecords={this.state.totalRecords}     
                        value={this.state.Students} 
                        selection={this.state.selectedStudents} 
                        onPage={e => this.onPage(e)}
                        onSelectionChange={(e) => this.setState({ selectedStudents: e.value })}
                        dataKey="id" paginator rows={this.state.rows}
                        paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink"
                        globalFilter={this.state.globalFilter}             
                        header={header}>

                        <Column selectionMode="multiple" headerStyle={{ width: '3rem' }}></Column>
                        <Column field="id" header="Id" style={{ width: "8rem" }} sortable></Column>
                        <Column field="name" header="Name" sortable></Column>
                        <Column field="surname" header="Surname" sortable></Column>
                        <Column field="age" header="Age" style={{ width: "8rem" }} sortable></Column>
                        <Column field="email" header="Email" style={{ width: "20rem" }} body={this.emailBodyTemplate} sortable></Column>
                        <Column field="registerDate" header="Register date" sortable></Column>
                        <Column field="dateOfCourseStart" header="Course start" sortable></Column>
                        <Column body={this.actionBodyTemplate}></Column>
                    </DataTable>
                </div>

                <Dialog visible={this.state.StudentDialog} style={{ width: '450px' }} header="Student Details" modal className="p-fluid" footer={StudentDialogFooter} onHide={this.hideDialog}>
                    <div className="p-field">
                        <label htmlFor="name">Name</label>
                        <InputText id="name" value={this.state.Student.name} onChange={(e) => this.onInputChange(e, 'name')} required autoFocus className={classNames({ 'p-invalid': this.state.submitted && !this.state.Student.name })} />
                        {this.state.submitted && !this.state.Student.name && <small className="p-invalid">Name is required.</small>}
                    </div>     
                    <div className="p-field">
                        <label htmlFor="surname">Surname</label>
                        <InputText id="surname" value={this.state.Student.surname} onChange={(e) => this.onInputChange(e, 'surname')} required autoFocus className={classNames({ 'p-invalid': this.state.submitted && !this.state.Student.surname })} />
                        {this.state.submitted && !this.state.Student.surname && <small className="p-invalid">Surname is required.</small>}
                    </div>               

                    <div className="p-field">
                        <label htmlFor="age">Age</label>
                        <InputNumber id="age" value={this.state.Student.age} onValueChange={(e) => this.onAgeChange(e, 'age')} integeronly />
                    </div>

                    <div className="p-field">
                        <label htmlFor="email">Email</label>
                        <InputText id="email" value={this.state.Student.email} onChange={(e) => this.onInputChange(e, 'email')} required autoFocus className={classNames({ 'p-invalid': this.state.submitted && !this.state.Student.email })} />
                        {this.state.submitted && !this.state.Student.email && <small className="p-invalid">Email is required.</small>}
                    </div>  
                
                </Dialog>

                <Dialog visible={this.state.deleteStudentDialog} style={{ width: '450px' }} header="Confirm" modal footer={deleteStudentDialogFooter} onHide={this.hideDeleteStudentDialog}>
                    <div className="confirmation-content">
                        <i className="pi pi-exclamation-triangle p-mr-3" style={{ fontSize: '2rem' }} />
                        {this.state.Student && <span>Are you sure you want to delete <b>{this.state.Student.name} {this.state.Student.surname}</b>?</span>}
                    </div>
                </Dialog>

                <Dialog visible={this.state.deleteStudentsDialog} style={{ width: '450px' }} header="Confirm" modal footer={deleteStudentsDialogFooter} onHide={this.hideDeleteStudentsDialog}>
                    <div className="confirmation-content">
                        <i className="pi pi-exclamation-triangle p-mr-3" style={{ fontSize: '2rem' }} />
                        {this.state.Student && <span>Are you sure you want to delete the selected students?</span>}
                    </div>
                </Dialog>
            </div>
         );
    }
}
 
const mapStateToProps = state => {
    return {
        studentReducer: get(state, 'AllStudents')
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        GetStudentsCount: () => {
            dispatch(reducer.GetStudentsCount());
        },
        GetAllStudents: (from,to) => {
            dispatch(reducer.GetAllStudents(from,to));
        },
        CreateStudent: (model) => {
            dispatch(reducer.CreateStudent(model));
        },
        EditStudent: (model) => {
            dispatch(reducer.EditStudent(model));
        },
        DeleteStudent: (model) => {
            dispatch(reducer.DeleteStudent(model));
        },
        ClearState: () => {
            dispatch(reducer.ClearState());
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(AllStudents);