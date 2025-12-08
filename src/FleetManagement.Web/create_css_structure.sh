#!/bin/bash

# Base directory (modify if needed)
ROOT="wwwroot/css"

# Create directory structure
mkdir -p $ROOT/base
mkdir -p $ROOT/modules/aircraft

# Create base CSS files
touch $ROOT/base/reset.css
touch $ROOT/base/variables.css
touch $ROOT/base/typography.css
touch $ROOT/base/layout.css

# Create aircraft module CSS files
touch $ROOT/modules/aircraft/aircraft.css
touch $ROOT/modules/aircraft/aircraft-sidebar.css
touch $ROOT/modules/aircraft/aircraft-forms.css

echo "CSS folder structure created successfully:"
echo "wwwroot/css/base/* and wwwroot/css/modules/aircraft/*"

