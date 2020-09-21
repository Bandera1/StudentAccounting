import axios from "axios";
import { serverUrl } from '../../../config';

export default class ConfirmEmailService {
    static ConfirmEmail(model) {
        return axios.post(`${serverUrl}api/account/confirmEmail`, model)
    };
}