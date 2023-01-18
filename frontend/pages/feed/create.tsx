import { useState } from "react";
import "@uiw/react-md-editor/markdown-editor.css";
import dynamic from "next/dynamic";
import { IncomingMessage, ServerResponse } from "http";
import { NextApiRequest, NextApiResponse } from "next";
import { AuthOptions, unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import { UserToken } from "../../types";
import { useSession } from "next-auth/react";
import axios from "axios";
import { useRouter } from "next/router";

const MDEditor = dynamic(
    () => import("@uiw/react-md-editor"),
    { ssr: false }
);


export default function CreateFeed() {
    const session = useSession();
    const router = useRouter();

    const [title, setTitle] = useState("");
    const [summary, setSummary] = useState("");
    const [image, setImage] = useState("");
    const [content, setContent] = useState<string | undefined>("");

    return (
        <div className="m-4 p-4 grid grid-cols-1 gap-4 mx-auto">
            <h1 className="text-4xl font-semibold text-center">Create blog</h1>
            <form>
                <div className="form-control w-full max-w-lg mx-auto">
                    <label className="label">
                        <span className="label-text">Title</span>
                    </label>
                    <input type="text" name='title' placeholder="Title" className="input input-bordered w-full max-w-lg" value={title} onChange={(e) => setTitle(e.target.value)} />
                </div>

                <div className="form-control w-full max-w-lg mx-auto">
                    <label className="label">
                        <span className="label-text">Summary</span>
                    </label>
                    <input type="text" name='summary' placeholder="Summary" className="input input-bordered w-full max-w-lg" value={summary} onChange={(e) => setSummary(e.target.value)} />
                </div>

                <div className="form-control w-full max-w-lg mx-auto">
                    <label className="label">
                        <span className="label-text">Image</span>
                    </label>
                    <input type="text" name='image' placeholder="Image Link" className="input input-bordered w-full max-w-lg" value={image} onChange={(e) => setImage(e.target.value)} />
                </div>

                <div className="form-control w-full max-w-lg mx-auto">
                    <label className="label">
                        <span className="label-text">Content</span>
                    </label>
                    <MDEditor preview="edit" value={content} onChange={setContent} className="textarea input-bordered w-full max-w-lg" />
                </div>

                <div className="form-control w-full max-w-lg mx-auto mt-4">
                    <button className="btn btn-primary" onClick={async (e) => {
                        e.preventDefault();
                        const data = {
                            token: session.data?.user?.image,
                            content,
                            summary,
                            photo: image,
                            title
                        }
                        
                        const feedReq = await axios.post('/api/feed', data);

                        const newId = feedReq.data.message.split(' ')[3];

                        router.push(`/feed/${newId}`);
                    }
                    }>Create</button>
                </div>
            </form>

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
                destination: '/auth/login'
            }
        }
    }

    return {
        props: {
            session
        }
    }
}