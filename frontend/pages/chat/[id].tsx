import { useRouter } from 'next/router'
import axios from 'axios';
import { AUTH_BASE_URL, ChatMessage, CHAT_BASE_URL, UserData } from '../../types';
import { unstable_getServerSession } from 'next-auth';
import { authOptions } from '../api/auth/[...nextauth]';
import { useState } from 'react';
import { useSession } from 'next-auth/react';
import { format } from 'date-fns';
import useSWR, { useSWRConfig } from 'swr';



interface ChatProps {
    myUser: UserData,
    otherUser: UserData
}

export default function Chat(props: ChatProps) {
    const { data: session, status } = useSession()
    const { mutate } = useSWRConfig()

    const fetcher = (url: string) => axios.get(url, {
        headers: {
            'Authorization': `Bearer ${session?.user?.image}`
        }
    }).then((res) => res.data).catch((e) => {
        return [];
    })

    const { data, error } = useSWR(
        `/api/messages/${props.otherUser.id}`,
        fetcher
    );

    const [chatMessage, setChatMessage] = useState('')

    return (
        <div className="m-4 p-8 grid grid-cols-1 gap-4 lg:w-1/2 mx-auto">
            <div className="">
                {data?.map((entry: ChatMessage) => {
                    if (entry.senderId === props.myUser.id) {
                        return (
                            <div className="chat chat-end">
                                <div className="chat-image avatar">
                                    <div className="w-10 rounded-full">
                                        <img src={props.myUser.thumbnailUrl} />
                                    </div>
                                </div>
                                <div className="chat-header">
                                    {props.myUser.firstName} {props.myUser.lastName}
                                    <time className="text-xs opacity-50"> {format(new Date(entry.createdAt), 'HH:mm')}</time>
                                </div>
                                <div className="chat-bubble">{entry.content}</div>
                            </div>
                        );
                    } else {
                        return (
                            <div className="chat chat-start">
                                <div className="chat-image avatar">
                                    <div className="w-10 rounded-full">
                                        <img src={props.otherUser.thumbnailUrl} />
                                    </div>
                                </div>
                                <div className="chat-header">
                                    {props.otherUser.firstName} {props.otherUser.lastName}
                                    <time className="text-xs opacity-50"> {format(new Date(entry.createdAt), 'HH:mm')}</time>
                                </div>
                                <div className="chat-bubble">{entry.content}</div>
                            </div>
                        );
                    }
                })}
            </div>
            <div className='flex'>
                <div className="form-control w-full max-w-xs mx-auto">
                    <input type="text" placeholder="Your message" className="inline input input-bordered w-full max-w-xs" value={chatMessage} onChange={(e) => setChatMessage(e.target.value)} />
                    <button className='m-4 btn btn-primary' onClick={async () => {
                        await axios.post(`/api/Chat/${props.otherUser.id}`, {
                            content: chatMessage
                        }, {
                            headers: {
                                'Authorization': `Bearer ${session?.user?.image}`
                            }
                        });
                        setChatMessage('');
                        mutate(`/api/messages/${props.otherUser.id}`)
                    }}>Send</button>
                </div>
            </div>
        </div>
    );
}

export async function getServerSideProps(context: { query: { id: string; }; }) {
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

    const id = context.query.id;

    const userResponse = await axios.get(`${AUTH_BASE_URL}/api/User/${id}`);

    const myResponse = await axios.get(`${AUTH_BASE_URL}/api/User/${session.user.name}`);

    return {
        props: {
            otherUser: { ...userResponse.data, id },
            myUser: { ...myResponse.data, id: session.user.name }
        }
    }
}