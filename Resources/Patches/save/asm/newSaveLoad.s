.thumb
push	{r4-r5,lr}
push	{r0-r7}
ldr	r0,=#0x2000004
ldr	r1,=#0x203FB00
ldrb	r0,[r0]
ldr	r2,=#0x500
mul	r0,r2
ldr	r3,=#0xE001100
add	r0,r3
mov	r3,#0
copyLoop:
ldrb	r4,[r0,r3]
strb	r4,[r1,r3]
add	r3,#1
cmp	r3,r2
bne	copyLoop
end:
pop	{r0-r7}
ldr	r0,=#0x3000FD0
ldrb	r5,[r0]
cmp	r5,#0
ldr	r3,=#0x80515F9
bx	r3
