import type { NextApiRequest, NextApiResponse } from 'next'
import axios from 'axios';
import { FEED_BASE_URL } from '../../types';

type Data = {
    message: string
}

// The files under /api will be used to proxy the requests to the microservices without CORS configuration
export default async function handler(
    req: NextApiRequest,
    res: NextApiResponse<Data>
) {
    if (req.method === 'POST') {
        try {
            const response = await axios.post(`${FEED_BASE_URL}/api/create_feed`, req.body);
            return res.status(201).send({
                message: response.data
            });
        } catch (error) {
            console.log(error);
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
