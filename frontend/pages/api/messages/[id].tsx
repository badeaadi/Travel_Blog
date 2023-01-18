import type { NextApiRequest, NextApiResponse } from 'next'
import axios from 'axios';
import { ChatMessage, CHAT_BASE_URL } from '../../../types';

type Data = {
    message: string
}

// The files under /api will be used to proxy the requests to the microservices without CORS configuration
export default async function handler(
    req: NextApiRequest,
    res: NextApiResponse<Data | ChatMessage[]>
) {
    if (req.method === 'GET') {
        try {
            const response = await axios.get(`${CHAT_BASE_URL}/api/Message/with/${req.query.id}`, {
                headers: {
                    'Authorization': req.headers["authorization"]
                }
            });
            return res.status(200).send(response.data);
        } catch (error) {
            res.status(500).send({
                message: error instanceof Error ? error.message : 'Internal server error'
            });
        }
    } else {
        res.status(405).send({
            message: 'Method not allowed'
        });
    }
}
