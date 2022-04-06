import axios from 'axios';
import { config } from '../env';
import https from 'https';

const getJWT = async  () => {
    const response = await axios.post(config.baseURL + '/Login', {
        eMail: config.email,
        password: config.password
    }, {
        httpsAgent: new https.Agent({
            rejectUnauthorized: false
        })
    })
    return response.data
}

export const createHTTPClient = async  () => {
    const jwt = await getJWT()

    const instance = axios.create({
        baseURL: config.baseURL,
        headers: {Authorization: jwt},
        httpsAgent: new https.Agent({
            rejectUnauthorized: false
        })
    });
    
    return instance;
}

