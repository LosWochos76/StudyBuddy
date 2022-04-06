import { AxiosInstance } from "axios";
import { createHTTPClient } from "../utils/signIn";

describe('NotificationUserMetadata', () => {
    let client: AxiosInstance;
    beforeAll(() => {
        return createHTTPClient().then( c =>  {
            client = c
        });
    });
    
    test('asd', async () => {
        const response = await client.get('/NotificationUserMetadata')

        expect(response.data).toBeInstanceOf(Array)
    })

    test('asd', async () => {
        
        const body = {
            
        }
        
        const response = await client.post('/NotificationUserMetadata')

        expect(response.data).toBeInstanceOf(Array)
    })

    
    
})