require 'rubygems'
require 'rake'

WINDOWS = ENV['OS'] == 'Windows_NT'
ROOT = File.expand_path(File.dirname(__FILE__))
SPECS_PATH = File.join(ROOT, 'specs', 'specs.csproj')

task :default => 'specs'

task :compile do
  system "dotnet build -c Release"
end

task :specs => :compile do
  system "dotnet test #{SPECS_PATH}"
end

task :pack => :specs do
  publish_directory = "#{ROOT}/.pack"  
  system "dotnet pack -o \"#{publish_directory}\""
end
