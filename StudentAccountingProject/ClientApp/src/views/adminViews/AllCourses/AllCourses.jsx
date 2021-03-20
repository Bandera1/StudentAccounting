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
import { FileUpload } from 'primereact/fileupload';
import { Rating } from 'primereact/rating';
import { Dropdown } from 'primereact/dropdown';
import { InputMask } from 'primereact/inputmask';
import { InputTextarea } from 'primereact/inputtextarea';


import get from "lodash.get";
import { connect } from "react-redux";
import * as reducer from './reducer';

import 'semantic-ui-css/semantic.min.css'
import './style.scss'
import 'primereact/resources/themes/saga-blue/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';


class AllCourses extends Component {
    emptyCourse = {
        id: '',
        name: '',
        description: '',
        author:null,
        photoPath: '',
        newPhotoBase64:'',
        rating: 0,
        dateOfStart:'',
        dateOfEnd: ''
    };


    constructor(props) {
        super(props);

        this.state = {
            Courses: [],
            Authors: [],
            CourseDialog: false,
            deleteCourseDialog: false,
            deleteCoursesDialog: false,
            Course: this.emptyCourse,
            selectedCourses: null,
            submitted: false,
            globalFilter: null,
            isLoadingPage: false,

            loading: false,
            first: 0,
            totalRecords: 0,
            rows: 6,

            isCourseCreateSuccess: false,
            isCourseDeleteSuccess: false,

            isCourseCreateFailed: false,
            isCourseDeleteFailed: false,

            createCourseErrorMessage: '',
            deleteCourseErrorMessage: '',
        };

        this.leftToolbarTemplate = this.leftToolbarTemplate.bind(this);
        this.rightToolbarTemplate = this.rightToolbarTemplate.bind(this);
        this.imageBodyTemplate = this.imageBodyTemplate.bind(this);
        this.emailBodyTemplate = this.emailBodyTemplate.bind(this);
        this.actionBodyTemplate = this.actionBodyTemplate.bind(this);
        this.ratingTemplate = this.ratingTemplate.bind(this);

        this.openNew = this.openNew.bind(this);
        this.hideDialog = this.hideDialog.bind(this);
        this.saveCourse = this.saveCourse.bind(this);
        this.confirmDeleteCourse = this.confirmDeleteCourse.bind(this);
        this.deleteCourse = this.deleteCourse.bind(this);
        this.exportCSV = this.exportCSV.bind(this);
        this.confirmDeleteSelected = this.confirmDeleteSelected.bind(this);
        this.deleteSelectedCourses = this.deleteSelectedCourses.bind(this);
        this.onAgeChange = this.onAgeChange.bind(this);
        this.onInputChange = this.onInputChange.bind(this);
        this.onInputNumberChange = this.onInputNumberChange.bind(this);
        this.hideDeleteCourseDialog = this.hideDeleteCourseDialog.bind(this);
        this.hideDeleteCoursesDialog = this.hideDeleteCoursesDialog.bind(this);
    }


    componentDidMount = () => {
        this.props.GetCourseCount();
        this.props.GetAuthors();
        setTimeout(x => { this.updateCourses(this.state.first, this.state.rows); }, 1000)
    };

    componentWillReceiveProps = (nextProps) => {
        //- Binding
        this.setState({
            Courses: nextProps.CourseReducer.getCourses.Courses,
            Authors: nextProps.CourseReducer.getAuthors.authors,
            loading: nextProps.CourseReducer.getCourses.loading,

            totalRecords: nextProps.CourseReducer.getCount.count.count,

            //------CourseCreate
            isCourseCreateSuccess: nextProps.CourseReducer.createCourse.success,
            isCourseCreateFailed: nextProps.CourseReducer.createCourse.failed,
            //------CourseDelete
            isCourseDeleteSuccess: nextProps.CourseReducer.deleteCourse.success,
            isCourseDeleteFailed: nextProps.CourseReducer.deleteCourse.failed,

            //------Errors
            createCourseErrorMessage: nextProps.CourseReducer.createCourse.error,
            deleteCourseErrorMessage: nextProps.CourseReducer.deleteCourse.error,
        });
    }

    updateCourses = (CurrentPage, PageSize, Filter) => {
        let myFirst = CurrentPage == 0 ? 1 : CurrentPage;
        if (myFirst % 2 == 0 && myFirst > 2) {
            myFirst = myFirst - 1;
        }

        if(myFirst >= 5) myFirst -=3;
        console.log(myFirst);

        let newModel = {
            paggination: {
                filter: {
                    searchCriteria: Filter
                },
                currentPage: myFirst,
                pageSize: PageSize
            }
        }

        this.props.GetAllCourses(newModel);
    }

    openNew() {
        console.log("open");
        this.setState({
            Course: this.emptyCourse,
            submitted: false,
            CourseDialog: true
        });
    }

    hideDialog() {
        this.setState({
            submitted: false,
            CourseDialog: false
        });
    }

    hideDeleteCourseDialog() {
        this.setState({ deleteCourseDialog: false });
    }

    hideDeleteCoursesDialog() {
        this.setState({ deleteCoursesDialog: false });
    }

    saveCourse() {
        let state = { submitted: true };

        if (this.state.Course.name.trim()) {
            let Course = { ...this.state.Course };
            if (this.state.Course.id) {
                // let editedCourse = {
                //     DTO: {
                //         id: Course.id,
                //         Name: Course.name,
                //         Surname: Course.surname,
                //         Age: Course.age.toString(),
                //         Email: Course.email
                //     }
                // };
                // console.log("Update Course", editedCourse);
                // this.props.EditCourse(editedCourse);
            }
            else {
                let myAuthor = {
                    name: Course.author.name.split(' ')[0],
                    surname: Course.author.name.split(' ')[1]
                }
                let newCourse = {
                    model: {
                        name: Course.name,
                        description: Course.description,
                        authorId: this.state.Authors.find((a) => a.name == myAuthor.name && a.surname == myAuthor.surname).id,
                        photoBase64: Course.newPhotoBase64,
                        DateOfStart: Course.dateOfStart,
                        DateOfEnd: Course.dateOfEnd,
                    }
                }
                console.log("Create Course", newCourse);
                this.props.CreateCourse(newCourse);
            }

            state = {
                ...state,
                CourseDialog: false,
                Course: this.emptyCourse
            };
        }

        this.setState(state);
    }

    confirmDeleteCourse(Course) {
        this.setState({
            Course,
            deleteCourseDialog: true
        });
    }

    deleteCourse() {
        let deleteCourse = this.state.Courses.filter(val => val.id === this.state.Course.id);
        let model = {
            model: {
                courses: [{
                    CourseId: deleteCourse[0].id
                }]
            }
        }
        console.log("Delete val", model);
        this.props.DeleteCourse(model);

        this.setState({
            deleteCourseDialog: false,
            product: this.emptyCourse
        });
    }

    findIndexById(id) {
        let index = -1;
        for (let i = 0; i < this.state.Courses.length; i++) {
            if (this.state.Courses[i].id === id) {
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
        this.setState({ deleteCoursesDialog: true });
    }

    deleteSelectedCourses() {
        let Courses = this.state.Courses.filter(val => this.state.selectedCourses.includes(val)).map(a => {
            return {
                CourseId: a.id
            }
        });
        let model = {
            model: {
                Courses: [
                    ...Courses
                ]
            }
        }

        console.log("Deleted Courses", model);
        this.props.DeleteCourse(model);

        this.setState({
            deleteCoursesDialog: false,
            selectedCourses: null
        });
    }

    onAgeChange(e) {
        let Course = { ...this.state.Course };
        Course['age'] = e.value;
        this.setState({ Course });
    }

    onInputChange(e, name) {        
        const val = (e.target && e.target.value) || '';
        let Course = { ...this.state.Course };
        Course[`${name}`] = val;

        this.setState({ Course });
    }

    onInputNumberChange(e, name) {
        const val = e.value || 0;
        let Course = { ...this.state.Course };
        Course[`${name}`] = val;

        this.setState({ Course });
    }

    leftToolbarTemplate() {
        return (
            <>
                <Button label="New" icon="pi pi-plus" className="p-button-success p-mr-2 mr-3" onClick={this.openNew} />
                <Button label="Delete" icon="pi pi-trash" className="p-button-danger" onClick={this.confirmDeleteSelected} disabled={!this.state.selectedCourses || !this.state.selectedCourses.length} />
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

    imageBodyTemplate(rowData) {
        return <img src={`/${rowData.photoPath}`} className="product-image" />
    }

    ratingTemplate(rowData) {
        return <Rating value={rowData.rating} readonly stars={5} cancel={false} />
    }

    emailBodyTemplate(rowData) {
        return <span className="Course-badge status-instock">{rowData.email}</span>;
    }

    actionBodyTemplate(rowData) {
        return (
            <>
                <Button icon="pi pi-trash" className="p-button-rounded p-button-warning" onClick={() => this.confirmDeleteCourse(rowData)} />
            </>
        );
    }

    onPage(event) {
        const { first } = event;
        setTimeout(() => {
            this.setState({ first });
            this.updateCourses(this.state.first, this.state.rows);
        }, 100);

    }

    onFiler(filter) {
        this.updateCourses(this.state.first, this.state.rows, filter);
    }

    imageUrlToBase64 = (imgUrl) => {
        var img = new Image();
        img.src = imgUrl;
        img.setAttribute('crossOrigin', 'anonymous');
        img.onload = (() => {
            var canvas = document.createElement("canvas");
            canvas.width = img.width;
            canvas.height = img.height;
            var ctx = canvas.getContext("2d");
            ctx.drawImage(img, 0, 0);
            var dataURL = canvas.toDataURL("image/png");
            dataURL.replace(/^data:image\/(png|jpg);base64,/, "");

            let Course = { ...this.state.Course };
            Course.newPhotoBase64 = dataURL;
            this.setState({ Course });
        });
    }

    //-----------------------TOASTS------------------------------
    //-------CreateCourse
    createCourseSuccessToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'success', summary: 'Successful', detail: 'Course Created', life: 3000 });
        window.location.reload();
    }

    createCourseFailedToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'error', summary: 'Failed', detail: this.state.createCourseErrorMessage, life: 3000 });
    }

    //-------DeleteCourse
    deleteCourseSuccessToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'success', summary: 'Successful', detail: 'Deleted', life: 3000 });
        window.location.reload();
    }
    deleteCourseFailedToast = () => {
        this.props.ClearState();
        this.toast.show({ severity: 'error', summary: 'Failed', detail: this.state.deleteCourseErrorMessage, life: 3000 });
    }

    //-----------------------------------------------------------

    render() {
        const header = (
            <div className="table-header">
                <h5 className="p-m-0">Courses</h5>
                <span className="p-input-icon-left">
                    <i className="pi pi-search" />
                    <InputText type="search" onInput={(e) => this.onFiler(e.target.value)} placeholder="Search..." />
                </span>
            </div>
        );
        const CourseDialogFooter = (
            <>
                <Button label="Cancel" icon="pi pi-times" className="p-button-text" onClick={this.hideDialog} />
                <Button label="Save" icon="pi pi-check" className="p-button-text" onClick={this.saveCourse} />
            </>
        );
        const deleteCourseDialogFooter = (
            <>
                <Button label="No" icon="pi pi-times" className="p-button-text" onClick={this.hideDeleteCourseDialog} />
                <Button label="Yes" icon="pi pi-check" className="p-button-text" onClick={this.deleteCourse} />
            </>
        );
        const deleteCoursesDialogFooter = (
            <>
                <Button label="No" icon="pi pi-times" className="p-button-text" onClick={this.hideDeleteCoursesDialog} />
                <Button label="Yes" icon="pi pi-check" className="p-button-text" onClick={this.deleteSelectedCourses} />
            </>
        );

        if (this.state.isCourseCreateSuccess) this.createCourseSuccessToast();
        if (this.state.isCourseCreateFailed) this.createCourseFailedToast();
        if (this.state.isCourseDeleteSuccess) this.deleteCourseSuccessToast();
        if (this.state.isCourseDeleteFailed) this.deleteCourseFailedToast();

        return (
            <div className="datatable-crud-demo p-3">
                <Toast ref={(el) => this.toast = el} />

                <div className="card">
                    <Toolbar className="p-mb-4" left={this.leftToolbarTemplate} right={this.rightToolbarTemplate}></Toolbar>

                    <DataTable ref={(el) => this.dt = el}
                        loading={this.state.loading}
                        lazy first={this.state.first}
                        paginator rows={this.state.rows}
                        totalRecords={this.state.totalRecords}
                        value={this.state.Courses}
                        selection={this.state.selectedCourses}
                        onPage={e => this.onPage(e)}
                        onSelectionChange={(e) => this.setState({ selectedCourses: e.value })}
                        dataKey="id"
                        paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink"
                        header={header}>

                        <Column selectionMode="multiple" headerStyle={{ width: '3rem' }}></Column>
                        <Column field="id" header="Id" style={{ width: "18rem" }} sortable></Column>
                        <Column field="name" header="Name" sortable></Column>
                        <Column field="author.name" header="Author" style={{ width: "8rem" }} sortable></Column>
                        <Column field="author.surname" style={{ width: "8rem" }}></Column>                     
                        <Column header="Image" body={this.imageBodyTemplate}></Column>
                        <Column header="Rating" body={this.ratingTemplate} sortable></Column>
                        <Column field="dateOfStart" header="Course start" sortable></Column>
                        <Column field="dateOfEnd" header="Course end" sortable></Column>
                        <Column body={this.actionBodyTemplate} style={{ width: "6rem" }}></Column>
                    </DataTable>
                </div>

                <Dialog visible={this.state.CourseDialog} style={{ width: '450px' }} header="Course Details" modal className="p-fluid" footer={CourseDialogFooter} onHide={this.hideDialog}>
                    <div className="p-field">
                        <label htmlFor="name">Name</label>
                        <InputText id="name" value={this.state.Course.name} onChange={(e) => this.onInputChange(e, 'name')} required autoFocus className={classNames({ 'p-invalid': this.state.submitted && !this.state.Course.name })} />
                        {this.state.submitted && !this.state.Course.name && <small className="p-invalid">Name is required.</small>}
                    </div>

                    <div className="p-field">
                        <label htmlFor="name">Description</label>
                        {/* <InputText id="name" value={this.state.Course.description} onChange={(e) => this.onInputChange(e, 'description')} required autoFocus className={classNames({ 'p-invalid': this.state.submitted && !this.state.Course.description })} /> */}
                        <InputTextarea id="name" value={this.state.Course.description} onChange={(e) => this.onInputChange(e, 'description')} required autoFocus className={classNames({ 'p-invalid': this.state.submitted && !this.state.Course.description })} />{
                        this.state.submitted && !this.state.Course.description && <small className="p-invalid">Description is required.</small>}
                    </div>

                    <div className="p-field">
                        <label htmlFor="Author">Author</label>
                        <Dropdown value={this.state.Course.author} options={this.state.Authors.map((e) => { return { name: e.name + " " + e.surname } })} onChange={(e) => this.onInputChange(e, 'author')} optionLabel="name" placeholder="Select a Author" className={classNames({ 'p-invalid': this.state.submitted && !this.state.Course.author })}/>
                        {this.state.submitted && !this.state.Course.author && <small className="p-invalid">Author is required.</small>}
                    </div>

                    <div className="p-field">
                        <label htmlFor="email">Course start</label>
                        <InputMask mask="99/99/9999" value={this.state.Course.dateOfStart} placeholder="dd/mm/yyyy" slotChar="dd/mm/yyyy" onChange={(e) => this.onInputChange(e, 'dateOfStart')} className={classNames({ 'p-invalid': this.state.submitted && !this.state.Course.dateOfStart })}></InputMask>
                        {this.state.submitted && !this.state.Course.dateOfStart && <small className="p-invalid">Date of start is required.</small>}
                    </div>

                    <div className="p-field">
                        <label htmlFor="email">Course end</label>
                        <InputMask mask="99/99/9999" value={this.state.Course.dateOfEnd} placeholder="dd/mm/yyyy" slotChar="dd/mm/yyyy" onChange={(e) => this.onInputChange(e, 'dateOfEnd')} className={classNames({ 'p-invalid': this.state.submitted && !this.state.Course.dateOfEnd })}></InputMask>
                        {this.state.submitted && !this.state.Course.dateOfEnd && <small className="p-invalid">Date of end is required.</small>}
                    </div>

                    <div className="p-field">
                        <label htmlFor="age">Image</label>
                        <FileUpload mode="basic" accept="image/*" maxFileSize={1000000} customUpload={true} onSelect={e => {this.imageUrlToBase64(e.files[0].objectURL)}}/>
                        {this.state.submitted && !this.state.Course.newPhotoBase64 && <small className="p-invalid">Image is required.</small>}
                    </div>
                </Dialog>

                <Dialog visible={this.state.deleteCourseDialog} style={{ width: '450px' }} header="Confirm" modal footer={deleteCourseDialogFooter} onHide={this.hideDeleteCourseDialog}>
                    <div className="confirmation-content">
                        <i className="pi pi-exclamation-triangle p-mr-3" style={{ fontSize: '2rem' }} />
                        {this.state.Course && <span>Are you sure you want to delete <b>{this.state.Course.name} {this.state.Course.surname}</b>?</span>}
                    </div>
                </Dialog>

                <Dialog visible={this.state.deleteCoursesDialog} style={{ width: '450px' }} header="Confirm" modal footer={deleteCoursesDialogFooter} onHide={this.hideDeleteCoursesDialog}>
                    <div className="confirmation-content">
                        <i className="pi pi-exclamation-triangle p-mr-3" style={{ fontSize: '2rem' }} />
                        {this.state.Course && <span>Are you sure you want to delete the selected courses?</span>}
                    </div>
                </Dialog>
            </div>
        );
    }
}

const mapStateToProps = state => {
    return {
        CourseReducer: get(state, 'AllCourses')
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        GetCourseCount: () => {
            dispatch(reducer.GetCourseCount());
        },
        GetAuthors: () => {
            dispatch(reducer.GetAuthors());
        },
        GetAllCourses: (model) => {
            dispatch(reducer.GetAllCourses(model));
        },
        CreateCourse: (model) => {
            dispatch(reducer.CreateCourse(model));
        },
        DeleteCourse: (model) => {
            dispatch(reducer.DeleteCourse(model));
        },
        ClearState: () => {
            dispatch(reducer.ClearState());
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(AllCourses);