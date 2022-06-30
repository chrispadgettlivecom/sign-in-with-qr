# Sign in with QR using Azure AD B2C

## Inspiration

Devices at airports and networks at hotels or other public spaces are convenient but, because they are shared, then they must be assumed to be comprised since they might be infected with malicious software.

## What it does

My project protects sign-ups and sign-ins for applications (e.g. a banking experience) that are accessed on an untrusted device (e.g. the airport device) or on an untrusted network (e.g. the hotel Wi-Fi) using a QR code-initiated authentication across devices and networks.

## How we built it

[This diagram](https://github.com/chrispadgettlivecom/sign-in-with-qr/blob/main/media/authentication-flow.jpg) describes the authentication flow.

The authentication flow, which is inspired by the OAuth 2.0 device authorization grant flow, includes:

* One Azure AD B2C custom policy called *B2C_1A_sign_in_with_qr* that's displayed on the untrusted device or network. It generates a QR code that's displayed in the browser on the untrusted device. This QR code is then scanned on the trusted device.
* Another Azure AD B2C custom policy called *B2C_1A_sign_in_with_device_user* that's displayed on the trusted device and network. It prompts users to sign up or sign in in the browser on the trusted device. This trusted sign-up or sign-in is then validated on the untrusted device.
* A backend API, built on .NET 6 and deployed on Azure App Service, that allows the custom policies to communicate with each other.

[This illustration](https://github.com/chrispadgettlivecom/sign-in-with-qr/blob/main/media/authentication-interface.jpg) describes the authentication interface.

## Challenges we ran into

I didn't have any immediate challenges but the following considerations are important for a production solution:

* Device code brute forcing
* User code brute forcing
* Remote phishing
* Session spying
* Non-visual code transmission (e.g. audio or Bluetooth Low Energy)

## Accomplishments that we're proud of

An authentication experience that's convenient and secure.

# What we learned

The Azure AD B2C platform is great to build custom, different experiences on for authentication scenarios such as this.

# What's next for Sign in with QR using Azure AD B2C

Share as an Azure AD B2C custom policy community sample that others can refer to for their own implementation.
