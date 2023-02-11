.thumb
sub	r0,#0x39
lsl	r0,#2
neg	r0,r0
strh	r0,[r4,#0x36]
ldrb	r0,[r4,#0x0A]
ldr	r3,=#0x809AF65
bx	r3
