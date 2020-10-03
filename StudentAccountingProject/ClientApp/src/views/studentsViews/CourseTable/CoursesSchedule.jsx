import React, { Component } from 'react';
import FullCalendar from '@fullcalendar/react'
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';

import axios from "axios";
import { serverUrl } from '../../../config';

import 'primereact/resources/primereact.min.css'


class CoursesSchedule extends Component {
  
    state = {
        options: {
            defaultView: 'dayGridMonth',
            defaultDate: '2017-02-01',
            header: {
                left: 'prev,next',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
            },
            timeZone: 'UTC',
        },
        events: [
            { title: 'event 1', date: '2020-10-07', color: 'red' },
            {
                title: 'event 2',
                start: '2020-10-07T12:30:00',
                end: "2020-10-10", url: '/',
            }
        ]
    };

    componentDidMount() {
        this.getSchedule();
    }

    getScheduleByApi = () => {
        return axios.get(`${serverUrl}api/Schedule/GetSchedule`)
    }

    getSchedule = () => {
        this.getScheduleByApi().then((response) => {
            this.setState({
                events: response.data.items
            })
        }).catch(err => {
            console.error("Schedule api error", err);
        });  
    }


    render() { 
        const { events,options } = this.state;

        return ( 
             <div>
                <div className="card mt-2 m-1 p-3">
                    <FullCalendar
                        plugins = {[dayGridPlugin, timeGridPlugin, interactionPlugin]}
                        events={events}
                        options={options}
                    />
                </div>
            </div>
         );
    }
}
 
export default CoursesSchedule;