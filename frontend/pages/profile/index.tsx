import { useRouter } from 'next/router'
import axios from 'axios';
import { AUTH_BASE_URL, UserData, UserToken } from '../../types';
import { getSession, useSession } from 'next-auth/react';
import { AuthOptions, unstable_getServerSession } from 'next-auth';
import { authOptions } from '../api/auth/[...nextauth]';
import { IncomingMessage, ServerResponse } from 'http';
import { NextApiRequest, NextApiResponse } from 'next';

export default function Profile(props: UserData) {
	return (
		<div className="m-4 p-8 grid grid-cols-1 gap-4 lg:w-1/2 mx-auto">
            <div className="flex">
                <div className="avatar">
                    <div className="w-12 rounded-full">
                        <img src={props.thumbnailUrl} />
                    </div>
                </div>
                <div className="ml-4">
                    <p className="font-semibold">{props.firstName} {props.lastName}</p>
                    <div>
                        <p className="inline font-thin text-sm align-text-bottom">{props.email}</p>
                    </div>
                </div>
            </div>
        </div>
	);
}

export async function getServerSideProps(context: { req: (IncomingMessage & { cookies: Partial<{ [key: string]: string; }>; }) | NextApiRequest | AuthOptions; res: ServerResponse<IncomingMessage> | NextApiResponse<any> | AuthOptions; }) {
    // @ts-expect-error
    const session = (await unstable_getServerSession(context.req, context.res, authOptions)) as SessionData

    if (!session) {
        return {
            redirect: {
                permanent: false,
                destination: '/404'
            }
        }
    }

    const userResponse = await axios.get(`${AUTH_BASE_URL}/api/User/${session.user.name}`);

    return {
        props: {
            session,
            ...userResponse.data
        }
    }
}