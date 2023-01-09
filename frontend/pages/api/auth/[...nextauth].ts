import NextAuth, { AuthOptions } from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";

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
        async authorize(credentials, req) {

            // Replace with micro call
            const user = {id: "1", firstName: "test", lastName: "testescu", email: "test@gmail.com"}

            if (user) {
                return user;
            } else {
                return null;
            }
        }
    })
  ],
  pages: {
    signIn: "/auth/login"
  }
}
export default NextAuth(authOptions)