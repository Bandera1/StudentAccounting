import axios from "axios";
import { serverUrl } from '../../../config';

export default class LoginService {
    static login(model) {
        return axios.post(`${serverUrl}api/account/login`, model)
    };
    static facebookLogin(model) {
        return axios.post(`${serverUrl}api/account/facebookLogin`,model)
    }
}