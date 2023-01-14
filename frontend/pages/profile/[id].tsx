import { useRouter } from 'next/router'
import axios from 'axios';
import { BASE_URL, UserData } from '../../types';

export default function Profile(props: UserData) {
    const router = useRouter()

	return (
		<div className="m-4 p-8 grid grid-cols-1 gap-4 lg:w-1/2 mx-auto">
            <div className="flex">
                <div className="avatar">
                    <div className="w-12 rounded-full">
                        <img src="https://placeimg.com/192/192/people" />
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

export async function getServerSideProps(context: { query: { id: string; }; }) {
    const id = context.query.id;

    const userResponse = await axios.get(`${BASE_URL}/api/User/${id}`);

    console.log(userResponse.data);

    return {
        props: {
            ...userResponse.data
        }
    }
}