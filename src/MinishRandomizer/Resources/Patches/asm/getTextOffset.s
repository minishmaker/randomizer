.thumb
push	{r4-r7,lr}
mov	r4,r0
mov	r5,r0
mov	r0,#0xFF
and	r4,r0		@id
lsr	r5,#8		@bank
ldr	r0,=#0x2000000
ldrb	r6,[r0,#7]	@language
cmp	r6,#1
bhi	noissuelang
mov	r6,#2
noissuelang:
@get banks table from language
ldr	r0,=#0x805E99C
ldr	r0,[r0]
lsl	r6,#2
add	r6,r0
ldr	r6,[r6]
@get ids table from bank
lsl	r5,#2
add	r5,r6
ldr	r5,[r5]
add	r5,r6
@get offset from ids table
lsl	r4,#2
add	r4,r5
ldr	r4,[r4]
add	r0,r4,r5
end:
pop	{r4-r7,pc}
