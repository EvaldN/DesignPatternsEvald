# Start Document

## Gym Workout Appplication

### Application Description

The Gym Workout Application is an app designed for users to receive a workout plan, based on their goals, weight and strength. It also allows the users to create their own workout plan and to measure their workout strategy, using measures, such as, sets and reps or time. The application will be made using C#.

### Features
_The functionalities are divided using MoSCoW analysis_
#### Must have
---
**Workout generation** - Ability to generate a workout of certain type by using the inputs of weight, strength and goals.

**Strategy adjustment** - Ability to adjust the workout stategy by set and reps or time.

**Workout builder** - Ability to build a workout of certain sphere, such as cardio or resistance.

**User Interface (UI)** - An UI through which the user can interact with the system.

#### Should have
---
**Excercise database** - A database attached to the application with various excercises in order to facilitate easier workout creation.

**Customization** - Ability to customize already existing workouts by adding or removing excercises.

#### Could have
---
**Motivational features** - Integration of motivational features such as achievement badges, inspirational quotes, or progress milestones to keep users motivated.

**Online availability** - Ability for the application to run online, not just locally.

#### Won't have
---
**Advanced analytics** - In-depth analytics and data visualization features for analyzing workout trends and patterns.

**Post-development support** - The application will not be updated after its development is finished.

### Design Patterns Used
##### Factory Method Pattern
**Purpose**: To generate different types of workout routines based on user preferences, such as cardio or resistance. This pattern solves the problem of creating the default workout routine dynamically based on user input.
##### Strategy Pattern
**Purpose**: To implement different strategies for progress tracking, such as tracking by sets and reps, tracking by time, or tracking by intensity. This pattern solves the problem of accommodating various progress tracking methods while keeping the application flexible and maintainable.
##### Builder Pattern
**Purpose**: To construct and customize workout routines by difficulty, allowing users to specify certain excercise difficulty based on thier input. This pattern solves the problem of creating personalized workout routines in a structured and cohesive manner while providing in-depth control to the user.
### Class Diagram
_To do_
