.thumb
ldrb	r0,[r4,#0x0A]
ldr	r1,table
mov	r2,#12
mul	r0,r2
ldrh	r0,[r1,r0]

ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800

ldr	r3,=#0x809AC61
bx	r3

.align
.ltorg
table:
