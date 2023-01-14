import NextAuth, { AuthOptions } from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";
import axios from 'axios';
import jwt from 'jsonwebtoken';
import { BASE_URL, UserToken } from "../../../types";

export const authOptions: AuthOptions = {
  providers: [
    CredentialsProvider({
        name: "Auth",
        credentials: {
            email: {
                label: "Email",
                type: "text"
            },
            password: {
                label: "Password",
                type: "text"
            }
        },
        async authorize(credentials) {
            try {
                const res: any = await axios.post(`${BASE_URL}/api/User/login`, credentials);
                const userData = (jwt.decode(res.data.token) as UserToken);

                // Cannot override returned data, therefore we need to use some fields as other fields
                const data = {
                    id: userData.id,
                    email: userData.email,
                    name: userData.id,
                    image: res.data.token
                }

                return data
            } catch (error) {
                return null;
            }
        }
    })
  ],
  pages: {
    signIn: "/auth/login"
  },
  callbacks: {
    async redirect() {
        return '/';
    },
  },
  secret: 'yes'
}
export default NextAuth(authOptions)