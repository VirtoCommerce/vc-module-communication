# Main Concept
This article describes the internal structure of the VirtoCommerce Communication Module that allows developers to create communication systems for their Virto-based applications.

## Overview
_VirtoCommerce Communication_ is a basic module for messaging, it does not contain an API for interacting with services, and a UI for displaying messages to users. Only a data storage system and services for working with them.

Centralized storage of messages allows for messaging regardless of which application the user uses. Separate implementation of the UI for communications in each application allows for flexible customization of the appearance and UX depending on the requirements of the project.

![VC Communication use case](media/01-communicaton-use-case-chart.png)

In this diagram:

- Black - the implemented message processing mechanism

- Green - an example of implementing communication between the Operator and the Vendor. The Operator works from his workplace based on the VirtoCommerce Platform, the Vendor uses communications on the Vendor Portal

- Orange - a potential scenario for adding messages for the Customer using the VirtoCommerce Frontend application

## Core Structure
