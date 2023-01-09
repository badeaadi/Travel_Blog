import type { NextApiRequest, NextApiResponse } from 'next'

type Data = {
  name: string
}

// The files under /api will be used to proxy the requests to the microservices without CORS configuration
export default function handler(
  req: NextApiRequest,
  res: NextApiResponse<Data>
) {
  res.status(200).json({ name: 'John Doe' })
}
