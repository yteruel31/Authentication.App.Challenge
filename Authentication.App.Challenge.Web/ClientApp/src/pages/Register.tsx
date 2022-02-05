import {mdiEmail, mdiLock, mdiGoogle, mdiFacebook, mdiTwitter, mdiGithub} from "@mdi/js";
import Icon from "@mdi/react";
import React from 'react';
import Button from "../components/Button";
import Input from "../components/Input";

const Register: React.FC = (props) => {
    const {} = props

    return (
        <div className="md:m-auto md:max-w-md">
            <div className="p-5 md:border-primary md:border-2 md:rounded-xl md:p-10">
                <div className="mb-5">
                    <p className="font-semibold mb-2 text-xl">
                        Join thousands of learners from around the world
                    </p>
                    <p>Master web development by making real-life projects. There are multiple paths for you to
                        choose</p>
                </div>
                <Input placeholder="Email" icon={mdiEmail}/>
                <Input placeholder="Password" icon={mdiLock}/>
                <Button className="w-full">Start coding now</Button>
                <p className="text-center my-5">or continue with these social profile</p>
                <div className="grid grid-flow-col">
                    <div className="flex justify-center">
                        <div
                            className="flex justify-center items-center border-2 border-text-secondary text-text-secondary rounded-full w-16 h-16">
                            <Icon path={mdiGoogle} size={1.5}/>
                        </div>
                    </div>
                    <div className="flex justify-center">
                        <div
                            className="flex justify-center items-center border-2 border-text-secondary text-text-secondary rounded-full w-16 h-16">
                            <Icon path={mdiFacebook} size={1.5}/>
                        </div>
                    </div>
                    <div className="flex justify-center">
                        <div
                            className="flex justify-center items-center border-2 border-text-secondary text-text-secondary rounded-full w-16 h-16">
                            <Icon path={mdiTwitter} size={1.5}/>
                        </div>
                    </div>
                    <div className="flex justify-center">
                        <div
                            className="flex justify-center items-center border-2 border-text-secondary text-text-secondary rounded-full w-16 h-16">
                            <Icon path={mdiGithub} size={1.5}/>
                        </div>
                    </div>
                </div>
                <p className="text-center mt-5">Adready a member? <a className="text-button" href="#">Login</a></p>
            </div>
        </div>
    );
};

export default Register;